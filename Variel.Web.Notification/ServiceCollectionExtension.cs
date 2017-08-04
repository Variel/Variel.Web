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
        public static IServiceCollection AddVarielSms(this IServiceCollection services)
        {
            services.AddScoped<SmsService, SmsService>();
            return services;
        }
    }
}
