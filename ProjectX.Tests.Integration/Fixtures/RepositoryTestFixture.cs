using System;
using System.Threading.Tasks;
using ProjectX.DataAccess.Context.Base;

namespace ProjectX.Tests.Integration.Fixtures
{
    public class RepositoryTestFixture : IDisposable, IAsyncDisposable
    {
        public RepositoryTestFixture()
        {
            DbFixture = new DbFixture();
            MongoContext = DbFixture.MongoContext;
        }
        
        public IMongoContext MongoContext { get; set; }
        public DbFixture DbFixture { get; set; }
        
        public async ValueTask DisposeAsync()
        {
            if (MongoContext is not null)
            {
                await MongoContext.DeleteDatabase();
            }
        }

        public async void Dispose() => await DisposeAsync();
    }
}