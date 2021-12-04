using Microsoft.Extensions.DependencyInjection;
using ProjectX.BusinessLayer.Services.Files;

namespace ProjectX.BusinessLayer.Services.DI
{
    public static class ConfigureBusinessServices
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<JwtGenerator>();
            services.AddScoped<GridFsFileService>();
            
            return services;
        }
    }
}