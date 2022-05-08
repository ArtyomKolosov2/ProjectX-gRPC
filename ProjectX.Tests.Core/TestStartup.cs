using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProjectX.DataAccess.DependencyInjection;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.Tests.Core
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureDb(IServiceCollection services)
        {
            var databaseSettings = new DatabaseSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = $"Database-ProjectX-{Guid.NewGuid()}"
            };
            
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            
            services.AddMongoDbContext(databaseSettings);
            services.AddMongoDbIdentity(databaseSettings);
        }
    }
}