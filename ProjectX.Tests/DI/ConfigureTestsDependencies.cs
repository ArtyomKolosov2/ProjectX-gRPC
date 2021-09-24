using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Tests.Helpers;

namespace ProjectX.Tests.DI
{
    public static class ConfigureTestsDependencies
    {
        public static void ConfigureTests(this IServiceCollection serviceCollection)
        {
            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            
            serviceCollection.AddScoped(_ => new ServiceResolverHelper(webHost));
        }
    }
}