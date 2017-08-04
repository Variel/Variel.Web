using Microsoft.Extensions.DependencyInjection;

namespace Variel.Web.Common
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVarielAppSettings<TContext>(this IServiceCollection services, TContext dbContext)
            where TContext : class, ISettingsDatabaseContext
        {
            services.AddScoped<ISettingsDatabaseContext>(_ => dbContext);
            services.AddScoped<SettingsProvider, SettingsProvider>();
            return services;
        }
    }
}
