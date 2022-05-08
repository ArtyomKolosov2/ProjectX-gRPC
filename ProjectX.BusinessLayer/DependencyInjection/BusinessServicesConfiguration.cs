using Microsoft.Extensions.DependencyInjection;
using ProjectX.BusinessLayer.Services;
using ProjectX.BusinessLayer.Services.Files;

namespace ProjectX.BusinessLayer.DependencyInjection
{
    public static class BusinessServicesConfiguration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<JwtGenerator>();
            services.AddScoped<GridFsFileService>();
            services.AddScoped<BusinessUserService>();
            
            return services;
        }
    }
}