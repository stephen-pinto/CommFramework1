using EasyRpc.Core.Client;
using EasyRpc.Types;
using Grpc.Core;

namespace EasyRpc.Peer.Net
{
    public class PeerNetService : PeerService.PeerServiceBase
    {
        private readonly MakeRequestDelegate _makeRequestHandler;
        private readonly NotifyDelegate _notifyHandler;

        public PeerNetService(MakeRequestDelegate makeRequestHandler, NotifyDelegate notifyHandler)
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
