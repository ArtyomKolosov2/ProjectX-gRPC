using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Repositories.Base;
using ProjectX.Tests.Integration.Base;
using ProjectX.Tests.Integration.Fixtures;
using ProjectX.Tests.Integration.TestContexts.Collections;
using Xunit;

namespace ProjectX.Tests.Integration.Repositories
{
    [Collection(nameof(RepositoryTestCollection))]
    public class BaseAbstractRepositoryTests : BaseRepositoryTests
    {
        private const string TestString = "Test";
        private readonly Repository<TestEntity> _testRepository;
        private readonly IMongoContext _mongoContext;
        
        public BaseAbstractRepositoryTests(RepositoryTestFixture testFixture) : base(testFixture)
        {
            _testRepository = new TestBaseRepository(testFixture.DbFixture.MongoContext);
            _mongoContext = TestFixture.DbFixture.MongoContext;
        }

        [Fact]
        public async Task GetAll_OneTestEntityCreatedAndInsertedToDb_EntityFromDbIsTheSame()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                IntValue = 1,
                ListOfStrings = new List<string>(Enumerable.Repeat(TestString, 3)),
                StringValue = TestString
            };

            // Act
            await _mongoContext.GetCollection<TestEntity>().InsertOneAsync(testEntity);

            var result = await _testRepository.GetAll();
            var entityFromDb = result.Single();

            // Assert
            entityFromDb.IntValue.Should().Be(testEntity.IntValue);
            entityFromDb.ListOfStrings.Should().Contain(testEntity.ListOfStrings);
            entityFromDb.StringValue.Should().Be(testEntity.StringValue);
        }

        [Fact]
        public async Task Insert_OneTestEntityCreated_EntityFromDbIsTheSame()
        {
            var testEntity = new TestEntity
            {
                IntValue = 1,
                ListOfStrings = new List<string>(Enumerable.Repeat(TestString, 3)),
                StringValue = TestString
            };

            await _testRepository.Insert(testEntity);
            
            var result = await _testRepository.GetAll();
            var entity = result.Single();

            entity.IntValue.Should().Be(testEntity.IntValue);
            entity.ListOfStrings.Should().Contain(testEntity.ListOfStrings);
            entity.StringValue.Should().Be(testEntity.StringValue);
        }

    }
}