using Microsoft.Extensions.DependencyInjection;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Context.DI
{
    public static class MongoDbContextConfiguration
    {
        public static IServiceCollection AddMongoDbContext(this IServiceCollection services, IDatabaseSettings settings)
        {
            services.AddSingleton<IMongoContext, MongoDbContext>(
                _ => new MongoDbContext(settings));

            return services;
        }
    }
}