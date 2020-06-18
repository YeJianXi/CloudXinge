# Cloud.Xinge
实现对Andriod和IOS的简单推送
其中Andriod使用了腾讯云的信鸽,IOS使用了PushSharp，源码地址：https://github.com/Redth/PushSharp
提供了.netcore主机的依赖注入方式

# 引入
using Cloud.Xinge;
#服务注册
services.AddXinge();

#调用
public class Person{
    public Persion(IPushService pushservice){
        pushervice.Send(msg);
    }
}

# 配置
{
"Xinge": {
    "PushAddres": "https://api.tpns.tencent.com/v3/push/app",
    "accessID": "xxxxxxx",
    "secretKey": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
  }
}

