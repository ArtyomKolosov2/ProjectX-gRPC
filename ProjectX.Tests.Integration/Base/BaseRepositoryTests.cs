using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Repositories.Base;
using ProjectX.Tests.Integration.Fixtures;
using Xunit;

namespace ProjectX.Tests.Integration.Base
{
    public class BaseRepositoryTests: IAsyncLifetime
    {
        private const string RolesCollectionName = "Roles";
        protected RepositoryTestFixture TestFixture { get; }

        public BaseRepositoryTests(RepositoryTestFixture testFixture)
        {
            TestFixture = testFixture;
        }
        
        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            var collectionNames = (await TestFixture.MongoContext.Database.ListCollectionNamesAsync()).ToEnumerable()
                .Except(new[] { RolesCollectionName });

            Task DeleteCollectionAction(string name)
            {
                var collection = TestFixture.MongoContext.Database.GetCollection<BsonDocument>(name);
                return collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
            }

            foreach (var collectionName in collectionNames)
            {
                await DeleteCollectionAction(collectionName);
            }
        }
        
        protected class TestEntity : Entity<ObjectId>
        {
            public string StringValue { get; set; }
            public int IntValue { get; set; }
            public List<string> ListOfStrings { get; set; }
        }
        
        protected class TestBaseRepository : Repository<TestEntity>
        {
            private const string CollectionName = nameof(TestEntity);
            public TestBaseRepository(IMongoContext mongoContext) : base(mongoContext, CollectionName)
            {
            }
        }
    }
}