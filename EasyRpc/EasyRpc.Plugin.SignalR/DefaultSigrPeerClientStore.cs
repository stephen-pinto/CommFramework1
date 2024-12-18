using EasyRpc.Core.Base;
using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRpc.Plugin.SignalR
{
    public class DefaultSigrPeerClientStore : ISigrPeerClientStore
    {
        private readonly SignalRPeerHub _hub;
        private readonly ResponseAwaiter _responseAwaiter;
        private readonly Dictionary<string, IPeerService> _clients;

        public DefaultSigrPeerClientStore(IServiceProvider serviceProvider)
        {
            _hub = serviceProvider.GetService<SignalRPeerHub>() ?? throw new TypeInitializationException("PeerHub not initialized", null);
            _responseAwaiter = serviceProvider.GetService<ResponseAwaiter>() ?? throw new TypeInitializationException("ResponseAwaiter not initialized", null);
            _clients = new Dictionary<string, IPeerService>();
        }

        public IPeerService GetClient(string connectionId)
        {
            return _clients[connectionId];
        }

        public IPeerService AddClient(string connectionId, RegistrationRequestSigr registration)
        {
            var client = new PeerSigrClient(_hub, registration, connectionId, _responseAwaiter);
            _clients.Add(connectionId, client);
            return client;
        }

        public void RemoveClient(string connectionId)
        {
            PeerSigrClient sigrPeerClient = (PeerSigrClient)_clients[connectionId];
            _clients.Remove(connectionId);
        }

        public RegistrationRequestSigr GetRegistration(string connectionId)
        {
            return ((PeerSigrClient)_clients[connectionId]).Registration;
        }
    }
}
