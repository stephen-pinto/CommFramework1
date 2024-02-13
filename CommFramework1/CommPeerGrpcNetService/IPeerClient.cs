using CommServices.CommShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommPeerGrpcNetService
{
    internal interface IPeerClient
    {
        Task<Message> MakeRequest(Message message);

        Task<Message> Notify(Message message);
    }
}
