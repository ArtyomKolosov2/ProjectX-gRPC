using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectX.Tests.Helpers
{
    public class ServiceResolverHelper
    {
        private readonly IWebHost _webHost;

        public ServiceResolverHelper(IWebHost webHost) => _webHost = webHost;

        public T GetService<T>()
        {
            using var serviceScope = _webHost.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            try
            {
                var scopedService = services.GetRequiredService<T>();
                return scopedService;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}