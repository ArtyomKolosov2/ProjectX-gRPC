using System.Threading.Tasks;
using FluentAssertions;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;
using ProjectX.Tests.Core.Extensions;
using ProjectX.Tests.Integration.Fixtures;
using Xunit;

namespace ProjectX.Tests.Integration.Base 
{
    public abstract class BaseIntegrationTests : IAsyncLifetime
    {
        protected const string TestEmail = "test1@mail.com";
        protected const string TestPassword = "123abcABC!";
        protected const string TestLogin = "Test";
        protected IntegrationTestFixture TestFixture { get; }

        protected BaseIntegrationTests(IntegrationTestFixture testFixture)
        {
            TestFixture = testFixture;
        }

        protected Task<RegisterReply> CreateDefaultTestUser() => CreateNewTestUser(TestEmail, TestPassword, TestLogin);

        protected async Task<RegisterReply> CreateDefaultTestUserWithValidReply()
        {
            var reply = await CreateDefaultTestUser();
            
            reply.ErrorMessages.Should().BeNullOrEmpty();
            reply.Token.Should().NotBeNullOrEmpty();
            reply.IsSuccess.Should().BeTrue();

            return reply;
        }

        private async Task<RegisterReply> CreateNewTestUser(string email, string password, string login)
        {
            // Arrange
            var registerService = new UserAuthentication.UserAuthenticationClient(TestFixture.TestGrpcChannel);
            
            // Act
            var reply = await registerService.RegisterAsync(new RegisterRequest
            {
                Email = TestEmail,
                Password = TestPassword,
                Login = TestLogin
            });

            return reply;
        }

        #region TestSetup

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => TestFixture.MongoContext.ClearCollectionsAfterTestRun();

        #endregion
        
    }
}