using System;
using ProjectX.DataAccess.Context;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models;

namespace ProjectX.Tests.Integration.Fixtures
{
    public class DbFixture
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
    }
}