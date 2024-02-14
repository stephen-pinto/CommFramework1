using CommMaster.PeerManagement;
using CommMaster.Exceptions;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace CommMaster
{
    public class PeerService : CommPeerService.CommPeerServiceBase
    {
        private readonly IPeerRegistry _clientRegistry;

        public PeerService(IPeerRegistry clientRegistry)
        {
            _clientRegistry = clientRegistry;
        }

        public override async Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            //TODO: Should context be passed to the peer handle?
            if (_clientRegistry.TryGetValue(request.To, out var client))
            {
                return await client.Handle.MakeRequest(request);
            }

            throw new ClientNotFoundException("Client not found");
        }

        public override async Task<Message> Notify(Message request, ServerCallContext context)
        {
            if (_clientRegistry.TryGetValue(request.To, out var client))
            {
                return await client.Handle.Notify(request);
            }

            throw new ClientNotFoundException("Client not found");
        }
    }
}
