using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Xinge
{
    public interface IPushService
    {
        Task<string> Send(Message message);

    }
}
