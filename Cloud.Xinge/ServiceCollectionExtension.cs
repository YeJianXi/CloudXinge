using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
namespace Cloud.Xinge
{
    public static class XingeServiceCollectionExtension
    {
        public static void AddXinge(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IPushService>(sp => {
                var config = sp.GetService<IConfiguration>();
                var xingeOption = config.GetSection(XingeOptions.Xinge).Get<XingeOptions>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var client = httpClientFactory.CreateClient();
                return new PushService(client, xingeOption);
            });
        }
    }
}
