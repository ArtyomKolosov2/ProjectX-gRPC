using Microsoft.Extensions.DependencyInjection;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Repositories.Base;

namespace ProjectX.DataAccess.Repositories.DI
{
    public static class RepositoriesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<HelloRequestEntity>, HelloRequestRepository>();
        }
    }
}