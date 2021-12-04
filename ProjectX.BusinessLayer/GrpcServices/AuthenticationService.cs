using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectX.BusinessLayer.Services;
using ProjectX.Core.Constants;
using ProjectX.DataAccess.Models.Identity;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;

namespace ProjectX.BusinessLayer.GrpcServices
{
    [AllowAnonymous]
    public class AuthenticationService : UserAuthentication.UserAuthenticationBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtGenerator _generator;

        public AuthenticationService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            JwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _generator = jwtGenerator;
        }

        public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                
                if (result.Succeeded)
                {
                    var token = _generator.CreateJwtToken(user, await _userManager.GetRolesAsync(user));
                    var response = new LoginReply
                    {
                        IsSuccess = true,
                        Token = token
                    };

                    return response;
                }
            }

            return new LoginReply
            {
                IsSuccess = false,
                ErrorMessages = {"Invalid credentials!"},
                Token = string.Empty
            };
        }
        
        public override async Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context)
        {
            var newUser = new User
            {
                UserName = request.Login,
                Email = request.Email,
            };

            var identityResult = await _userManager.CreateAsync(newUser, request.Password);
            
            if (identityResult.Succeeded)
            {
                await _signInManager.CheckPasswordSignInAsync(newUser, request.Password, false);
                await _userManager.AddToRoleAsync(newUser, IdentityRoleConstants.User);

                var token = _generator.CreateJwtToken(newUser, await _userManager.GetRolesAsync(newUser));
                var response = new RegisterReply
                {
                    IsSuccess = true,
                    Token = token
                };

                return response;
            }
            
            var reply = new RegisterReply
            {
                IsSuccess = false,
                ErrorMessages = { identityResult.Errors.Select(x => x.Description) },
                Token = string.Empty
            };

            return reply;
        }
    }
}