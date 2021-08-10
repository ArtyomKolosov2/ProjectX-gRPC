using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;

namespace ProjectX.BusinessLayer.GrpcServices
{
    public class AuthenticationService : UserAuthentication.UserAuthenticationBase
    {
        public AuthenticationService()
        {
            
        }
        
        public override Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            return base.Login(request, context);
        }

        public override Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context)
        {
            return base.Register(request, context);
        }

        public override Task<Empty> Logout(Empty request, ServerCallContext context)
        {
            return base.Logout(request, context);
        }
    }
}