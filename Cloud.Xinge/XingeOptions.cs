using System;
using System.Collections.Generic;
using System.Text;

namespace Cloud.Xinge
{
    public class XingeOptions
    {

        public const string Xinge = "Xinge";

        public string PushAddres { get; set; } = "https://api.tpns.tencent.com/v3/push/app";

        public string accessID { get; set; } 
        
        public string secretKey { get; set; }


        public string cetificate { get; set; }

        public string cetificateSecret { get; set; }

        public int environment { get; set; }



    }
}
