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
        private const string KeyPrefix = "Client_";

        public DefaultSigrPeerClientStore(IMemoryCache memoryCache, SignalRPeerHub hub, ResponseAwaiter responseAwaiter)
        {
            _hub = hub;
            _memoryCache = memoryCache;
            _responseAwaiter = responseAwaiter;
        }

        public IPeerService GetClient(string connectionId)
        {
            return GetCachedClient(connectionId);
        }

        public IPeerService AddClient(string connectionId, RegistrationRequestSigr registration)
        {
            var client = new PeerSigrClient(_hub, registration, connectionId, _responseAwaiter);
            _memoryCache.Set(KeyPrefix + connectionId, client);
            return client;
        }

        public void RemoveClient(string connectionId)
        {
            _memoryCache.Remove(KeyPrefix + connectionId);
        }

        public RegistrationRequestSigr GetRegistration(string connectionId)
        {
            return GetCachedClient(connectionId).Registration!;
        }

        private PeerSigrClient GetCachedClient(string connectionId)
        {
            return (PeerSigrClient)_memoryCache.Get<IPeerService>(KeyPrefix + connectionId)!;
        }

        public IPeerService UpdateClient(string connectionId, RegistrationRequestSigr registration)
        {
            var client = GetCachedClient(connectionId);
            client.Registration = registration;
            RemoveClient(connectionId);
            AddClient(connectionId, registration);
            return client;
        }
    }
}
