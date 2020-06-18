using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Cloud.Xinge;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace XingeV3Demo
{

    public class MyHostService : IHostedService
    {
        IPushService pushService;
        public MyHostService(IPushService pushService)
        {
            this.pushService = pushService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            //var msg = new Cloud.Xinge.AndriodMessage()
            //{
            //    message_type = Cloud.Xinge.message_type.notify,
            //    audience_type = Cloud.Xinge.audience_type.token_list,
            //    Title = "hi,andriod",
            //    Content = "hello,master"
            //};

            //msg.SetShowType(false);
            //msg.SetActionType(3);
            //msg.SetVibrate(false);
            //msg.SetTokenList(new List<string>() { "0301aa2b23b23e5c7f7184cc35fd711fc600" });
            //msg.SetIntent("http://m.sxkid.com");
            //var custom = new Dictionary<string, object>();
            //custom.Add("key", "value");
            //msg.SetCustomContent(custom);
            //pushService.Send(msg);




            var iosmsg = new IOSMessage()
            {
                message_type = Cloud.Xinge.message_type.notify,
                audience_type = Cloud.Xinge.audience_type.token_list,
                Title = "hi,ios",
                Content = "hello,master",
            };
            iosmsg.SetBadge(-2);
            iosmsg.SetButtonTitles(new[] { "确定", "取消" });
            iosmsg.SetUrls(new[] { "http://m.sxkid.com" });
            iosmsg.SetTokenList(new List<string>() { "7a580b2676601294e025ea247e153f71594a2b9e66511b35817efd639be31ec6" });
            var result = pushService.Send(iosmsg).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {

            Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
            {
                services.AddHostedService<MyHostService>();
                services.AddXinge();


            }).Build().Run();


        }




      
    }
}
