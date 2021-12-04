using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;
using ProjectX.Tests.Integration.TestContext;
using Xunit;

namespace ProjectX.Tests.Integration.Base 
{
    public abstract class BaseIntegrationTests : IAsyncLifetime
    {
        protected const string TestEmail = "test1@mail.com";
        protected const string TestPassword = "123abcABC!";
        protected const string TestLogin = "Test";
        private const string RolesCollectionName = "Roles";
        protected IntegrationTestContext TestContext { get; }

        protected BaseIntegrationTests(IntegrationTestContext testContext)
        {
            TestContext = testContext;
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
        
        protected async Task<RegisterReply> CreateNewTestUser(string email, string password, string login)
        {
            // Arrange
            var registerService = new UserAuthentication.UserAuthenticationClient(TestContext.TestGrpcChannel);
            
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

        public async Task DisposeAsync()
        {
            var collectionNames = (await TestContext.MongoContext.Database.ListCollectionNamesAsync()).ToEnumerable()
                .Except(new[] { RolesCollectionName });

            Task DeleteCollectionAction(string name)
            {
                var collection = TestContext.MongoContext.Database.GetCollection<BsonDocument>(name);
                return collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
            }

            foreach (var collectionName in collectionNames)
            {
                await DeleteCollectionAction(collectionName);
            }
        }

        #endregion
        
    }
}