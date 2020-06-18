# Cloud.Xinge
腾讯云信鸽SDK(非官方)
提供了.netcore主机的依赖注入方式

# 调用
using Cloud.Xinge;
services.AddXinge();

# 配置
{
"Xinge": {
    "PushAddres": "https://api.tpns.tencent.com/v3/push/app",
    "accessID": "xxxxxxx",
    "secretKey": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
  }
}

