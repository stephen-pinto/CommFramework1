using EasyRpc.Core.Base;
using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Core.Util;
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
        private readonly IPeerClientResolver _resolver;
        private readonly ICertificateProvider _serverCertificateProvider;
        private readonly List<IEasyRpcPlugin> _plugins;
        public event EventHandler<Message>? Notification;
        public event EventHandler<PeerInfo>? PeerAdded;
        public event EventHandler? PeerRemoved;

        //TODO: This will be used to indetify what targets we want to communicate with and their addr.
        public IReadOnlyCollection<PeerInfo> PeerList => _registry.Values.Select(x => x.Peer).ToList();

        public string Id { get; } = Guid.NewGuid().ToString();

        public EasyRpcService(Uri uri, IPeerClientResolver peerClientResolver)
        {
            _serviceHost = uri.Host;
            _port = uri.Port;
            _registry = new PeerRegistry();
            _resolver = peerClientResolver;
            _serverCertificateProvider = new DefaultServerCertificateProvider();
            _plugins = new List<IEasyRpcPlugin>();
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            request.RegistrationId = Guid.NewGuid().ToString();

            var handle = _resolver.GetHandle(request);
            var entry = new PeerRegistryEntry(request.ToPeer(), handle);

            _registry.Add(request.RegistrationId, entry);

            Debug.WriteLine($"Registered peer {request.Name} with id {request.RegistrationId}");

            PeerAdded?.Invoke(this, entry.Peer);

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

            PeerRemoved?.Invoke(this, EventArgs.Empty);

            return await Task.FromResult(new RegistrationResponse
            {
                RegistrationId = request.RegistrationId,
                Status = "Success"
            });
        }

        public async Task<Empty> Notify(Message message)
        {
            if (_registry.ContainsKey(message.From))
                Notification?.Invoke(this, message);

            return await Task.FromResult<Empty>(new());
        }

        public async Task<Message> MakeRequest(Message message)
        {
            PeerNotFoundException.ThrowIfNullOrEmpty(message.To);

            //TODO: Should context be passed to the peer handle?
            if (_registry.TryGetValue(message.To, out var client))
                return await client.Handle.MakeRequest(message).ConfigureAwait(false);

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
