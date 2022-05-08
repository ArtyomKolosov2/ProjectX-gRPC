using System;
using System.Threading.Tasks;
using ProjectX.DataAccess.Context.Base;
using ProjectX.Tests.Core.Extensions;

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
            await MongoContext.DeleteDatabase();
        }

        public async void Dispose() => await DisposeAsync();
    }
}