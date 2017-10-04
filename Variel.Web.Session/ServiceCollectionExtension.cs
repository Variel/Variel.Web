using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Variel.Web.Session;

namespace Variel.Web.Common
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVarielSession<TAccount>(this IServiceCollection services, Action<SessionOptions> configure = null)
            where TAccount : class, IAccount
        {
            services.AddDistributedMemoryCache();

            if (configure != null)
                services.AddSession(configure);
            else
                services.AddSession();

            services.AddScoped<SessionService<TAccount>, SessionService<TAccount>>();

            return services;
        }
    }
}
