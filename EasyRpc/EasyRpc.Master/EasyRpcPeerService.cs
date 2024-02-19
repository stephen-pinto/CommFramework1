using EasyRpc.Core.Client;
using EasyRpc.Peer;
using EasyRpc.Types;
using Grpc.Core;

namespace EasyRpc.Master
{
    public class EasyRpcPeerService : PeerService.PeerServiceBase
    {
        private readonly MakeRequestDelegate _makeRequestHandler;
        private readonly NotifyDelegate _notifyHandler;

        public EasyRpcPeerService(MakeRequestDelegate makeRequestHandler, NotifyDelegate notifyHandler)
        {
            _makeRequestHandler = makeRequestHandler;
            _notifyHandler = notifyHandler;
        }

        public override async Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            return await _makeRequestHandler(request).ConfigureAwait(false);
        }

        public override async Task<Empty> Notify(Message request, ServerCallContext context)
        {
            return await _notifyHandler(request).ConfigureAwait(false);
        }
    }
}
