using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ProjectX.BusinessLayer.Services
{
    public class JwtTokenValidatorService : ISecurityTokenValidator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenValidatorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "your string",
                ValidAudience = "your string",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtTokenKey"]))
            };

            var claimsPrincipal = handler.ValidateToken(securityToken, tokenValidationParameters, out validatedToken);
            return claimsPrincipal;
        }

        public bool CanValidateToken => true;
        public int MaximumTokenSizeInBytes { get; set; } = int.MaxValue;
    }
}