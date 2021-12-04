using Microsoft.Extensions.DependencyInjection;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Models.Files;
using ProjectX.DataAccess.Repositories.Base;
using ProjectX.DataAccess.Repositories.Files;

namespace ProjectX.DataAccess.Repositories.DI
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<HelloRequestEntity>, HelloRequestRepository>();
            services.AddScoped<IRepository<FileRecord>, FileRecordRepository>();

            return services;
        }
    }
}