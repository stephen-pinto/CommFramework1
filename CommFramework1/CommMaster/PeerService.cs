using CommMaster.ClientManagement;
using CommMaster.Exceptions;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace CommMaster
{
    internal class PeerService : CommPeerService.CommPeerServiceBase
    {
        private readonly IClientRegistry _clientRegistry;

        public PeerService(IClientRegistry clientRegistry)
        {
            _clientRegistry = clientRegistry;
        }

        public override Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            //TODO: Should context be passed to the peer handle?
            if(_clientRegistry.TryGetValue(request.To, out var client))
            {
                return client.Handle.MakeRequest(request);
            }

            return new Task<Message>(() => throw new ClientNotFoundException("Client not found"));
        }

        public override Task<Message> Notify(Message request, ServerCallContext context)
        {
            if (_clientRegistry.TryGetValue(request.To, out var client))
            {
                return client.Handle.MakeRequest(request);
            }

            return new Task<Message>(() => throw new ClientNotFoundException("Client not found"));
        }
    }
}
