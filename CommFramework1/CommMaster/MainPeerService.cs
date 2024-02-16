using CommPeerServices.Base.Client;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace CommMaster
{
    public class MainPeerService : CommPeerService.CommPeerServiceBase
    {
        private readonly MakeRequestDelegate _makeRequestHandler;
        private readonly NotifyDelegate _notifyHandler;

        public MainPeerService(MakeRequestDelegate makeRequestHandler, NotifyDelegate notifyHandler)
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
