using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.DataAccess.Context.Base;
using ProjectX.Tests.Core.ApplicationFactory;

namespace ProjectX.Tests.Integration.Fixtures
{
    public class IntegrationTestFixture: IDisposable, IAsyncDisposable
    {
        private readonly TestApplicationFactory<Startup> _webApplicationFactory;
        private readonly HttpClient _httpClient;
        private readonly TestServer _server;
        public GrpcChannel TestGrpcChannel { get; }
        public IMongoContext MongoContext { get; }

        public IntegrationTestFixture()
        {
            _webApplicationFactory = new TestApplicationFactory<Startup>();
            _httpClient = _webApplicationFactory.CreateDefaultClient();
            _server = _webApplicationFactory.Server;
            TestGrpcChannel = GrpcChannel.ForAddress(_httpClient.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = _httpClient
            });
            MongoContext = _webApplicationFactory.Services.GetService<IMongoContext>();
        }

        public async ValueTask DisposeAsync()
        {
            if (MongoContext is not null)
            {
                await MongoContext.DeleteDatabase();
            }
            
            _webApplicationFactory?.Dispose();
            _httpClient?.Dispose();
            TestGrpcChannel?.Dispose();
            _server?.Dispose();
        }

        public async void Dispose() => await DisposeAsync();
    }
}