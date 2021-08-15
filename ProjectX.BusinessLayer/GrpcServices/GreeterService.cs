using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Repositories.Base;
using ProjectX.Protobuf.Protos;
using HelloReply = ProjectX.Protobuf.Protos.HelloReply;

namespace ProjectX.BusinessLayer.GrpcServices
{
    [Authorize]
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IRepository<HelloRequestEntity> _repository;

        public GreeterService(ILogger<GreeterService> logger, IRepository<HelloRequestEntity> repository)
        {
            _logger = logger;
            _repository = repository;
        }
        
        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            await _repository.Insert(new HelloRequestEntity
            {
                Message = request.Name
            });
            
            return await Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}