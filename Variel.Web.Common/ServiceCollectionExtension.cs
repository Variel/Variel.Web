using Microsoft.Extensions.DependencyInjection;

namespace Variel.Web.Common
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVarielAppSettings<TContext, TAccount>(this IServiceCollection services)
            where TContext : class, ISettingsDatabaseContext, IAccountDatabaseContext<TAccount>
            where TAccount : class, IAccount
        {
            services.AddScoped<ISettingsDatabaseContext>(provider => provider.GetService<TContext>());
            services.AddScoped<SettingsProvider, SettingsProvider>();
            return services;
        }

        public static IServiceCollection AddVarielAccount<TContext, TAccount>(this IServiceCollection services)
            where TContext : class, ISettingsDatabaseContext, IAccountDatabaseContext<TAccount>
            where TAccount : class, IAccount
        {
            services.AddScoped<IAccountDatabaseContext<TAccount>>(provider => provider.GetService<TContext>());
            return services;
        }
    }
}
