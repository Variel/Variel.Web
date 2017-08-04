using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVarielAuthentication<TContext, TAccount>(this IServiceCollection services, TContext dbContext)
            where TAccount : class, IAccount
            where TContext : AuthenticationDatabaseContext<TAccount>
        {
            services.AddScoped<AuthenticationDatabaseContext<TAccount>>(_ => dbContext);
            return services;
        }
    }
}
