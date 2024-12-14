using EasyRpc.Core.Client;
using EasyRpc.Core.Util;
using EasyRpc.Core.Plugin;
using EasyRpc.Master.Exceptions;
using EasyRpc.Master.Extensions;
using EasyRpc.Master.PeerManagement;
using EasyRpc.Types;
using System.Diagnostics;

namespace EasyRpc.Master
{
    public partial class EasyRpcService : IEasyRpcServicesPluginAbstraction
    {
        private readonly IPeerRegistry _registry;
        private readonly IPeerMapper _peerMapper;
        private readonly IPeerClientResolver _resolver;
        private readonly ICertificateProvider _serverCertificateProvider;
        private readonly List<IEasyRpcPlugin> _plugins;
        public event EventHandler<Message>? Notification;

        public EasyRpcService(string serviceHost, int port, IPeerClientResolver peerClientResolver)
        {
            _serviceHost = serviceHost;
            _port = port;
            _registry = new PeerRegistry();
            _resolver = peerClientResolver;
            _serverCertificateProvider = new DefaultServerCertificateProvider();
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

        public Task<Empty> Notify(Message message)
        {
            if (_registry.ContainsKey(message.From))
            {
                Notification?.Invoke(this, message);
            }

            return Task.FromResult(new Empty());
        }

        public async Task<Message> MakeRequest(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.To))
            {
                if (!_peerMapper.HasAnyCriteria)
                    throw new PeerNotFoundException("Could not find peer");
            }

            //TODO: Should context be passed to the peer handle?
            if (_registry.TryGetValue(message.To, out var client))
            {
                return await client.Handle.MakeRequest(message).ConfigureAwait(false);
            }

            throw new PeerNotFoundException("Client not found");
        }

        public IEasyRpcServices UsePlugin(IEasyRpcPlugin plugin)
        {
            _plugins.Add(plugin);
            _resolver.AddFactory(plugin.TypeIdentifier, plugin.GetClientFactory());
            return this;
        }
    }
}
