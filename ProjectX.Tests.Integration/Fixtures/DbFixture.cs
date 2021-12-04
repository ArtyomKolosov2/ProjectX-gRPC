using System;
using System.Threading.Tasks;
using ProjectX.DataAccess.Context;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models;
using Xunit;

namespace ProjectX.Tests.Integration.Fixtures
{
    public class DbFixture : IDisposable, IAsyncDisposable
    {
        public IMongoContext MongoContext { get; }
        
        public DbFixture()
        {
            var databaseSettings = new DatabaseSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = $"Database-ProjectX-{Guid.NewGuid()}"
            };

            MongoContext = new MongoDbContext(databaseSettings);
        }
        
        public async void Dispose() => await DisposeAsync();

        public async ValueTask DisposeAsync() => await MongoContext.DeleteDatabase();

    }
}