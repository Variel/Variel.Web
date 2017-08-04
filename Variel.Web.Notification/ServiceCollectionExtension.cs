using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Variel.Web.Common;
using Variel.Web.Notification.Sms;

namespace Variel.Web.Notification
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVarielSms<TContext>(this IServiceCollection services, TContext dbContext)
            where TContext : class, ISettingsDatabaseContext
        {
            services.AddScoped<SmsService, SmsService>();
            return services;
        }
    }
}
