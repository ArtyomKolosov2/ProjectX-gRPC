using System.Threading.Tasks;
using FluentAssertions;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;
using ProjectX.Tests.Integration.Base;
using ProjectX.Tests.Integration.Fixtures;
using ProjectX.Tests.Integration.TestContexts.Collections;
using Xunit;

namespace ProjectX.Tests.Integration.GrpcServices
{
    [Collection(nameof(IntegrationTestCollection))]
    public class AuthenticationServiceTests : BaseIntegrationTests
    {
        private readonly UserAuthentication.UserAuthenticationClient _client;

        public AuthenticationServiceTests(IntegrationTestFixture testFixture) : base(testFixture)
        {
            _client = new UserAuthentication.UserAuthenticationClient(TestFixture.TestGrpcChannel);
        }
        
        [Fact]
        public async Task CreateNewUser_UserAlreadyRegistered_Failed()
        {
            // Arrange
            var expectedErrorMessages = new[]
            {
                $"Username '{TestLogin}' is already taken.",
                $"Email '{TestEmail}' is already taken."
            };
            
            // Act
            await CreateDefaultTestUserWithValidReply();
            var reply = await CreateDefaultTestUser();
            
            // Assert
            reply.IsSuccess.Should().BeFalse();
            reply.ErrorMessages.Should().Contain(expectedErrorMessages);
            reply.Token.Should().BeNullOrEmpty();
        }

        [Fact]
        public Task CreateNewUser_UserNotExistInDatabase_Success() => CreateDefaultTestUserWithValidReply();

        [Fact]
        public async Task Login_UserExists_Success()
        {
            // Arrange
            await CreateDefaultTestUser();

            // Act
            var reply = await _client.LoginAsync(new LoginRequest
            {
                Email = TestEmail,
                Password = TestPassword
            });
            
            // Assert
            reply.ErrorMessages.Should().BeNullOrEmpty();
            reply.IsSuccess.Should().BeTrue();
            reply.Token.Should().NotBeNullOrEmpty();
        }
    }
}