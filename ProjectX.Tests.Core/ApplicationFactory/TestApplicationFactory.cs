using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectX.DataAccess.Helpers.Identity;
using ProjectX.DataAccess.Models.Identity;

namespace ProjectX.Tests.Core.ApplicationFactory
{
    public class TestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class 
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseStartup<TestStartup>();
                    x.UseTestServer();
                    x.ConfigureTestServices(ConfigureTestDb);
                });
            
            return builder;
        }
        
        private static async void ConfigureTestDb(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var scopedServices = scope.ServiceProvider;
            try
            {
                var roleManager = scopedServices.GetRequiredService<RoleManager<Role>>();
                await IdentityRolesInitializer.EnsureStandardRolesCreated(roleManager);
            }
            catch (Exception ex)
            {
                var logger = scopedServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during server start!");
            }
        }
    }
}