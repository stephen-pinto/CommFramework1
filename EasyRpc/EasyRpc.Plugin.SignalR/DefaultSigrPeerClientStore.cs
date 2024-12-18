using EasyRpc.Core.Base;
using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using Microsoft.Extensions.Caching.Memory;

namespace EasyRpc.Plugin.SignalR
{
    public class DefaultSigrPeerClientStore : ISigrPeerClientStore
    {
        private readonly SignalRPeerHub _hub;
        private readonly ResponseAwaiter _responseAwaiter;
        private readonly IMemoryCache _memoryCache;
        private const string Key = "ClientStore";

        public DefaultSigrPeerClientStore(IMemoryCache memoryCache, SignalRPeerHub hub, ResponseAwaiter responseAwaiter)
        {
            _hub = hub;
            _memoryCache = memoryCache;
            _responseAwaiter = responseAwaiter;

            if (!memoryCache.TryGetValue(Key, out Dictionary<string, IPeerService>? _))
                memoryCache.Set(Key, new Dictionary<string, IPeerService>());
        }

        public IPeerService GetClient(string connectionId)
        {
            var clients = _memoryCache.Get<Dictionary<string, IPeerService>>(Key)!;
            return clients[connectionId];
        }

        public IPeerService AddClient(string connectionId, RegistrationRequestSigr registration)
        {
            var clients = _memoryCache.Get<Dictionary<string, IPeerService>>(Key)!;
            var client = new PeerSigrClient(_hub, registration, connectionId, _responseAwaiter);
            clients.Add(connectionId, client);
            _memoryCache.Set(Key, clients);
            return client;
        }

        public void RemoveClient(string connectionId)
        {
            var clients = _memoryCache.Get<Dictionary<string, IPeerService>>(Key)!;
            PeerSigrClient sigrPeerClient = (PeerSigrClient)clients[connectionId];
            clients.Remove(connectionId);
            _memoryCache.Set(Key, clients);
        }

        public RegistrationRequestSigr GetRegistration(string connectionId)
        {
            var clients = _memoryCache.Get<Dictionary<string, IPeerService>>(Key)!;
            return ((PeerSigrClient)clients[connectionId]).Registration;
        }
    }
}
