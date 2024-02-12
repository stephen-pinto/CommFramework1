using CommServices.CommShared;
using Grpc.Core;

namespace CommMaster
{
    internal class PeerService : CommServices.CommPeer.CommPeerService.CommPeerServiceBase
    {
        public override Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            return base.MakeRequest(request, context);
        }

        public override Task<Message> Notify(Message request, ServerCallContext context)
        {
            return base.Notify(request, context);
        }
    }
}
