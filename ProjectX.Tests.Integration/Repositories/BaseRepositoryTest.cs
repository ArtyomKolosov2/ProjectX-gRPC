using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Repositories.Base;
using ProjectX.Tests.Integration.Base;
using ProjectX.Tests.Integration.Fixtures;
using ProjectX.Tests.Integration.TestContext;
using ProjectX.Tests.Integration.TestContext.Collections;
using Xunit;

namespace ProjectX.Tests.Integration.Repositories
{
    [Collection(nameof(RepositoryTestCollection))]
    public class BaseAbstractRepositoryTests : BaseRepositoryTests
    {
        private readonly Repository<TestEntity> _testRepository;
        private readonly IMongoDatabase _database;
        
        public BaseAbstractRepositoryTests(RepositoryTestContext testContext) : base(testContext)
        {
            _testRepository = new TestBaseRepository(testContext.DbFixture.MongoContext);
            _database = TestContext.DbFixture.MongoContext.Database;
        }

        [Fact]
        public async Task GetAll()
        {
            var testEntity = new TestEntity
            {
                IntValue = 1,
                ListOfStrings = new List<string>{ "Test" },
                StringValue = "Test"
            };

            await _database.GetCollection<TestEntity>(nameof(TestEntity))
                .InsertOneAsync(testEntity);

            var result = await _testRepository.GetAll();
            var firstEntity = result.First();

            firstEntity.IntValue.Should().Be(testEntity.IntValue);
            firstEntity.ListOfStrings.Should().Contain(testEntity.ListOfStrings);
            firstEntity.StringValue.Should().Be(testEntity.StringValue);
        }

        
    }
}