using CommPeerServices.Base.Client;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace GrpcNetPeer
{
    public class PeerService : CommPeerService.CommPeerServiceBase
    {
        private readonly MakeRequestDelegate _makeRequestHandler;
        private readonly NotifyDelegate _notifyHandler;

        public PeerService(MakeRequestDelegate makeRequestHandler, NotifyDelegate notifyHandler)
        {
            _makeRequestHandler = makeRequestHandler;
            _notifyHandler = notifyHandler;
        }

        public override async Task<Message> MakeRequest(Message message, ServerCallContext context)
        {
            return await _makeRequestHandler(message);
        }

        public override async Task<Empty> Notify(Message message, ServerCallContext context)
        {
            return await _notifyHandler(message);
        }
    }
}
