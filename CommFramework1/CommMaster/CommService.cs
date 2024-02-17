using CommMaster.Exceptions;
using CommMaster.Extensions;
using CommMaster.PeerManagement;
using CommPeerServices.Base.Client;
using CommServices.CommMaster;
using CommServices.CommShared;
using System.Diagnostics;

namespace CommMaster
{
    public partial class CommService : IMasterClient, IPeerClient
    {
        private readonly IPeerRegistry _registry;
        private readonly IPeerMapper _peerMapper;
        private readonly IPeerClientResolver _resolver;

        public CommService(string serviceHost, int port, IPeerClientResolver peerClientResolver)
        {
            _serviceHost = serviceHost;
            _port = port;
            _registry = new PeerRegistry();
            _peerMapper = new PeerMapper();
            _peerMapper.AddCriteria(new DefaultPeerMappingCriteria(_registry));
            _resolver = peerClientResolver;
        }

        public async Task<RegisterationResponse> Register(RegisterationRequest request)
        {
            request.RegistrationId = Guid.NewGuid().ToString();

            var handle = _resolver.GetHandle(request);

            _registry.Add(request.RegistrationId, new PeerRegistryEntry(request.ToPeer(), handle));

            Debug.WriteLine($"Registered peer {request.Name} with id {request.RegistrationId}");

            //FIXME: Return another id as regid beacause it can be misidentified by another client.
            //Possibly try to use jwt token for registration id which can also be used for secure communication.
            return await Task.FromResult(new RegisterationResponse
            {
                RegistrationId = request.RegistrationId,
                Status = "Success"
            });
        }

        public async Task<RegisterationResponse> Unregister(RegisterationRequest request)
        {
            _registry.Remove(request.RegistrationId);

            Debug.WriteLine($"Unregistered peer with id {request.RegistrationId}");

            return await Task.FromResult(new RegisterationResponse
            {
                RegistrationId = request.RegistrationId,
                Status = "Success"
            });
        }

        public async Task<Message> MakeRequest(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.To))
            {
                if (!_peerMapper.HasAnyCriteria)
                    throw new PeerNotFoundException("Could not find peer");

                //If we have a mapping criteria defined then map accordingly
                var peer = _peerMapper.MapPeer(_registry[message.From].Peer);
                message.To = peer.Id;
            }

            //TODO: Should context be passed to the peer handle?
            if (_registry.TryGetValue(message.To, out var client))
            {
                return await client.Handle.MakeRequest(message).ConfigureAwait(false);
            }

            throw new PeerNotFoundException("Client not found");
        }

        public async Task<Empty> Notify(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.To))
            {
                if (!_peerMapper.HasAnyCriteria)
                    throw new PeerNotFoundException("Could not find peer");

                //If we have a mapping criteria defined then map accordingly
                var peer = _peerMapper.MapPeer(_registry[message.From].Peer);
                message.To = peer.Id;
            }

            if (_registry.TryGetValue(message.To, out var client))
            {
                return await client.Handle.Notify(message).ConfigureAwait(false);
            }

            throw new PeerNotFoundException("Peer not found");
        }
    }
}
