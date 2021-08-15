using Microsoft.Extensions.DependencyInjection;

namespace ProjectX.BusinessLayer.Services.DI
{
    public static class ConfigureBusinessServices
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<JwtGenerator>();
        }
    }
}