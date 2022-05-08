using Microsoft.Extensions.DependencyInjection;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Models.Files;
using ProjectX.DataAccess.Repositories;
using ProjectX.DataAccess.Repositories.Base;

namespace ProjectX.DataAccess.DependencyInjection
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<HelloRequestEntity>, Repository<HelloRequestEntity>>();
            services.AddScoped<IRepository<FileRecord>, Repository<FileRecord>>();
            services.AddScoped<IRepository<BusinessUser>, Repository<BusinessUser>>();

            return services;
        }
    }
}