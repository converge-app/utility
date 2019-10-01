using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Application.Utility.Database
{
    public static class MongoDbContext
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            var databaseSettings = new DatabaseSettings();
            databaseSettings.ReadFromEnvironment();
            var config = databaseSettings.GetConfiguration();

            services.Configure<DatabaseSettings>(config);

            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            return services;
        }
    }
}