using EasyRpc.Core.Base;
using EasyRpc.Core.Client;
using EasyRpc.Master;
using EasyRpc.Plugin.SignalR.Interfaces;

namespace EasyRpc.Plugin.SignalR
{
    public class SigrPeerClientFactory : IPeerClientFactory
    {
        private readonly ISigrPeerClientStore _clientStore;

        public SigrPeerClientFactory(ISigrPeerClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public IPeerService GetHandle(RegistrationRequest registerationRequest)
        {
            var client = _clientStore.GetClient(registerationRequest.Properties[CommonConstants.SigrReferenceTag]);
            return client;
        }
    }
}
