using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Master.Exceptions;
using EasyRpc.Master.Extensions;
using EasyRpc.Master.PeerManagement;
using EasyRpc.Types;
using System.Diagnostics;

namespace EasyRpc.Master
{
    public partial class EasyRpcService : IEasyRpcServices
    {
        private readonly IPeerRegistry _registry;
        private readonly IPeerMapper _peerMapper;
        private readonly IPeerClientResolver _resolver;
        private readonly List<IEasyRpcPlugin> _plugins;

        public EasyRpcService(string serviceHost, int port, IPeerClientResolver peerClientResolver)
        {
            _serviceHost = serviceHost;
            _port = port;
            _registry = new PeerRegistry();
            _peerMapper = new PeerMapper();
            _peerMapper.AddCriteria(new DefaultPeerMappingCriteria(_registry));
            _resolver = peerClientResolver;
            _plugins = new List<IEasyRpcPlugin>();
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            request.RegistrationId = Guid.NewGuid().ToString();

            var handle = _resolver.GetHandle(request);

            _registry.Add(request.RegistrationId, new PeerRegistryEntry(request.ToPeer(), handle));

            Debug.WriteLine($"Registered peer {request.Name} with id {request.RegistrationId}");

            //FIXME: Return another id as regid beacause it can be misidentified by another client.
            //Possibly try to use jwt token for registration id which can also be used for secure communication.
            return await Task.FromResult(new RegistrationResponse
            {
                RegistrationId = request.RegistrationId,
                Status = "Success"
            });
        }

        public async Task<RegistrationResponse> Unregister(RegistrationRequest request)
        {
            _registry.Remove(request.RegistrationId);

            Debug.WriteLine($"Unregistered peer with id {request.RegistrationId}");

            return await Task.FromResult(new RegistrationResponse
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

        public void UsePlugin(IEasyRpcPlugin plugin)
        {
            _plugins.Add(plugin);
            _resolver.AddFactory(plugin.TypeIdentifier, plugin.GetClientFactory());
        }
    }
}
