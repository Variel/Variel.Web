using Microsoft.Extensions.DependencyInjection;

namespace Variel.Web.Common
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVarielAppSettings<TContext>(this IServiceCollection services)
            where TContext : class, ISettingsDatabaseContext
        {
            services.AddScoped<ISettingsDatabaseContext>(provider => provider.GetService<TContext>());
            services.AddScoped<SettingsProvider, SettingsProvider>();
            return services;
        }
    }
}
