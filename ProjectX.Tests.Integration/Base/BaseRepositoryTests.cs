using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Repositories.Base;
using ProjectX.Tests.Integration.TestContext;

namespace ProjectX.Tests.Integration.Base
{
    public class BaseRepositoryTests // : IAsyncLifetime
    {
        protected RepositoryTestContext TestContext { get; }

        public BaseRepositoryTests(RepositoryTestContext testContext)
        {
            TestContext = testContext;
        }
        
        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task DisposeAsync()
        {
            throw new System.NotImplementedException();
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