using EasyRpc.Core.Client;
using EasyRpc.Types;
using Grpc.Core;

namespace EasyRpc.Peer.Net
{
    public class PeerNetService : PeerService.PeerServiceBase
    {
        private readonly MakeRequestDelegate _makeRequestHandler;

        public PeerNetService(MakeRequestDelegate makeRequestHandler)
        {
            _makeRequestHandler = makeRequestHandler;
        }

        public override async Task<Message> MakeRequest(Message message, ServerCallContext context)
        {
            return await _makeRequestHandler(message);
        }
    }
}
