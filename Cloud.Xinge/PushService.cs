using Microsoft.Extensions.Options;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Xinge
{
    public class PushService : IPushService
    {
        HttpClient client;
        XingeOptions option;

        public PushService(HttpClient httpClient, XingeOptions option)
        {
            if (option == null) {
                throw new Exception("未配置XingeOption");
            }
            this.option = option;
            this.client = httpClient;

        }


        public async Task<string> Send(Message message)
        {
            if (message is AndriodMessage)
            {
                string body = message.ToJson();
                HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");
                var timstamp = ToolHelper.GetUnixTimeStamp();
                string waitSign = $"{timstamp}{this.option.accessID}{body}";
                string hexSignCode = ToolHelper.ToHexString(ToolHelper.HMac_Sha256(waitSign, this.option.secretKey));
                string sign = Convert.ToBase64String(Encoding.Default.GetBytes(hexSignCode)); //生成签名
                httpContent.Headers.Add("AccessId", this.option.accessID);
                httpContent.Headers.Add("TimeStamp", timstamp.ToString());
                httpContent.Headers.Add("Sign", sign);
                var response = await client.PostAsync(this.option.PushAddres, httpContent);
                return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
            else if (message is IOSMessage)
            {
                var ios = message.GetDic("ios", message.message);
                return ToolHelper.SendIOSNotify(message.GetTokenList(), ios, (ApnsConfiguration.ApnsServerEnvironment)option.environment, AppDomain.CurrentDomain.BaseDirectory + "/" + option.cetificate, option.cetificateSecret);
                
            }
            return "undefine this message";
         
        }


       



    }
}
