using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cloud.Xinge
{
    internal class ToolHelper
    {

        /// <summary>
        /// 字节转16进制字符
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bs)
        {
            StringBuilder temp = new StringBuilder();
            foreach (var b in bs)
            {
                var hex = string.Format("{0:x2}", b);
                temp.Append(hex);
            }
            return temp.ToString();
        }


        /// <summary>
        /// Unix时间戳是使用世界统一时间UTC与1970年1月1号0时0分0秒做差取相差值，这里差值精度只计算到秒
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTimeStamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public static byte[] HMac_Sha256(string waitSign, string scretKey)
        {
            try
            {
                byte[] keyBytes = Encoding.Default.GetBytes(scretKey);
                byte[] contentBytes = Encoding.Default.GetBytes(waitSign);
                using (HMACSHA256 mySHA256 = new HMACSHA256(keyBytes))
                {
                    var resultBytes = mySHA256.ComputeHash(contentBytes);
                    return resultBytes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }


        public static string SendIOSNotify(List<string> devicetokens, IDictionary<string, object> ios, ApnsConfiguration.ApnsServerEnvironment environment, string cetificatePath, string cetificatePwd)
        {
            List<int> result = new List<int>();
            var config = new ApnsConfiguration(environment, cetificatePath, cetificatePwd);
            var broker = new ApnsServiceBroker(config);
            StringBuilder erromsg = new StringBuilder();
            // Wire up events
            broker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException notificationException)
                    {
                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;
                        result.Add((int)statusCode);
                    }
                    else
                    {

                        // Inner exception might hold more useful information like an ApnsConnectionException			
                        result.Add(10086);
                        Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            broker.OnNotificationSucceeded += (notification) =>
            { result.Add(0); };


            // Start the broker
            broker.Start();
            foreach (var deviceToken in devicetokens)
            {
                var paload = Newtonsoft.Json.JsonConvert.SerializeObject(ios);
                broker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,
                    Payload = JObject.Parse(paload),
                    Expiration = DateTime.Now.AddDays(2)
                });
            }
            broker.Stop();
            var response = new { ret_code = 0, result = Newtonsoft.Json.JsonConvert.SerializeObject(result) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(response);

        }

    }
}
