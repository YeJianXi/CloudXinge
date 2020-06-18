using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloud.Xinge
{

    public enum audience_type
    {
        all,
        token,
        token_list
    }
    public enum message_type
    {
        notify,
        message
    }

    public abstract class Message
    {
        protected IDictionary<string, object> PostBodyDic { get; set; } = new Dictionary<string, object>();
       
        public Message()
        {
            this.PostBodyDic["message"] = new Dictionary<string, object>();
        }


        /// <summary>
        /// 必须参数 audience_type
        /// </summary>
        public audience_type audience_type { set { this.PostBodyDic["audience_type"] = value.ToString(); } }

        /// <summary>
        /// 必须参数 message_type
        /// </summary>
        public message_type message_type { set { this.PostBodyDic["message_type"] = value.ToString(); } }

        /// <summary>
        /// 必须参数 message
        /// </summary>
        public IDictionary<string, object> message
        {
            get { return this.PostBodyDic["message"] as IDictionary<string, object>; }
        }

        /// <summary>
        /// 必须参数 Title
        /// </summary>
        public virtual  string Title { set { this.message["title"] = value; } }

        /// <summary>
        /// 必须参数 Content
        /// </summary>
        public virtual string Content { set { this.message["content"] = value; } }



        /// <summary>
        /// 设置可选参数 token_list
        /// </summary>
        /// <param name="tokens"></param>
        public void SetTokenList(List<string> tokens)
        {
            this.PostBodyDic["token_list"] = tokens;
        }


        public List<string> GetTokenList()
        {
           return this.PostBodyDic["token_list"] as List<string>;
        }

        /// <summary>
        /// 设置可选参数 expire_time
        /// </summary>
        /// <param name="second"></param>
        public void SetExpireTime(int second)
        {
            this.PostBodyDic["expire_time"] = second;
        }

        /// <summary>
        /// 设置可选参数 send_time
        /// </summary>
        /// <param name="second"></param>
        public void SetSendTime(DateTime time)
        {
            this.PostBodyDic["send_time"] = time.ToString("yyyy-MM-DD HH:MM:SS");
        }

        

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.PostBodyDic);
        }

        public IDictionary<string, Object> GetDic(string key, IDictionary<string, object> dic)
        {
            IDictionary<string, object> newdic = new Dictionary<string, object>();
            if (dic != null)
            {
                if (dic.TryGetValue(key, out object oldic))
                {
                    newdic = oldic as IDictionary<string, object>;
                }
                else
                {
                    newdic = new Dictionary<string, object>();
                }
            }
            return newdic;

        }



    }

    public class AndriodMessage : Message
    {
        public AndriodMessage()
        { }
        /// <summary>
        /// 可选参数 show_type，设置应用前台时，是否展示通知 
        /// </summary>
        public void SetShowType(bool show)
        {
            IDictionary<string, object> andriod_dic = this.GetDic("andriod", this.message);
            andriod_dic["show_type"] = show ? 2 : 1;
            this.message["andriod"] = andriod_dic;
        }

        /// <summary>
        /// 可选参数 ring，设置是否有铃声
        /// </summary>
        public void SetRing(bool ring)
        {
            IDictionary<string, object> andriod_dic = this.GetDic("andriod", this.message);
            andriod_dic["ring"] = ring ? 1 : 0;
            this.message["andriod"] = andriod_dic;
        }

        /// <summary>
        /// 可选参数 lights，设置是否亮呼吸灯
        /// </summary>
        public void SetLights(bool lights)
        {
            IDictionary<string, object> andriod_dic = this.GetDic("andriod", this.message);
            andriod_dic["lights"] = lights ? 1 : 0;
            this.message["andriod"] = andriod_dic;
        }


        /// <summary>
        /// 可选参数 vibrate，设置是否震动
        /// </summary>
        public void SetVibrate(bool vibrate)
        {
            IDictionary<string, object> andriod_dic = this.GetDic("andriod", this.message);
            andriod_dic["vibrate"] = vibrate ? 1 : 0;
            this.message["andriod"] = andriod_dic;
        }


        public void SetActionType(int actionType)
        {
            IDictionary<string, object> andriod_dic = this.GetDic("andriod", this.message);
            IDictionary<string, object> action_dic = this.GetDic("action", andriod_dic);
            action_dic["action_type"] = actionType;
            andriod_dic["action"] = action_dic;
            this.message["andriod"] = andriod_dic;
        }
        public void SetIntent(string intent)
        {
            IDictionary<string, object> andriod_dic = this.GetDic("andriod", this.message);
            IDictionary<string, object> action_dic = this.GetDic("action", andriod_dic);
            action_dic["intent"] = intent;
            andriod_dic["action"] = action_dic;
            this.message["andriod"] = andriod_dic;
        }
        public void SetCustomContent(IDictionary<string, object> dic)
        {
            IDictionary<string, object> andriod_dic = this.GetDic("andriod", this.message);
            andriod_dic["custom_content"] = dic;
            this.message["andriod"] = andriod_dic;
        }

    }

    public class IOSMessage : Message
    {
        public override string Title { set{
                IDictionary<string, object> ios_dic = this.GetDic("ios", this.message);
                IDictionary<string, object> aps_dic = this.GetDic("aps", ios_dic);
                IDictionary<string, object> alert_dic = this.GetDic("alert", aps_dic);
                alert_dic["title"] = value;
                aps_dic["alert"] = alert_dic;
                ios_dic["aps"] = aps_dic;
                ios_dic["title"] = value;
                this.message["ios"] = ios_dic;
            }
        }


        public override string Content { set {
                IDictionary<string, object> ios_dic = this.GetDic("ios", this.message);
                IDictionary<string, object> aps_dic = this.GetDic("aps", ios_dic);
                IDictionary<string, object> alert_dic = this.GetDic("alert", aps_dic);
                alert_dic["body"] = value;
                aps_dic["alert"] = alert_dic;
                ios_dic["aps"] = aps_dic;
                ios_dic["content"] = value;
                this.message["ios"] = ios_dic;

            } }

        public void SetUrls(string[] urls)
        {
            IDictionary<string, object> ios_dic = this.GetDic("ios", this.message);
            ios_dic["urls"] = urls;
            this.message["ios"] = ios_dic;
        }

        public void SetButtonTitles(string[] titles)
        {
            IDictionary<string, object> ios_dic = this.GetDic("ios", this.message);
            ios_dic["buttontitles"] = titles;
            this.message["ios"] = ios_dic;
        }

        public void SetBadge(int bt)
        {
            IDictionary<string, object> ios_dic = this.GetDic("ios", this.message);
            IDictionary<string, object> aps_dic = this.GetDic("aps",ios_dic);
            aps_dic["badge"] = bt;
            ios_dic["aps"] = aps_dic;
            this.message["ios"] = ios_dic;


        }

     
    }









}
