using CommMaster.Exceptions;
using CommMaster.PeerManagement;
using CommPeerServices.Base.Client;
using CommPeerServices.Base.Server;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace CommMaster
{
    public class MainPeerService : CommPeerService.CommPeerServiceBase, IPeerService
    {
        private readonly IPeerRegistry _clientRegistry;
        private readonly IPeerMapper _peerMapper;

        public MainPeerService(IPeerRegistry clientRegistry, IPeerMapper peerMapper)
        {
            _clientRegistry = clientRegistry;
            _peerMapper = peerMapper;
        }

        public override async Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.To))
            {
                if (!_peerMapper.HasAnyCriteria)
                    throw new PeerNotFoundException("Could not find peer");

                //If we have a mapping criteria defined then map accordingly
                var peer = _peerMapper.MapPeer(_clientRegistry[request.From].Peer);
                request.To = peer.Id;
            }

            //TODO: Should context be passed to the peer handle?
            if (_clientRegistry.TryGetValue(request.To, out var client))
            {
                return await client.Handle.MakeRequest(request).ConfigureAwait(false);
            }

            throw new PeerNotFoundException("Client not found");
        }

        public override async Task<Empty> Notify(Message request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.To))
            {
                if (!_peerMapper.HasAnyCriteria)
                    throw new PeerNotFoundException("Could not find peer");

                //If we have a mapping criteria defined then map accordingly
                var peer = _peerMapper.MapPeer(_clientRegistry[request.From].Peer);
                request.To = peer.Id;
            }

            if (_clientRegistry.TryGetValue(request.To, out var client))
            {
                return await client.Handle.Notify(request).ConfigureAwait(false);
            }

            throw new PeerNotFoundException("Peer not found");
        }

        async Task<Message> IPeerClient.MakeRequest(Message message)
        {
            return await MakeRequest(message, null!).ConfigureAwait(false);
        }

        async Task<Empty> IPeerClient.Notify(Message message)
        {
            return await Notify(message, null!).ConfigureAwait(false);
        }
    }
}
