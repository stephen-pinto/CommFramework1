using EasyRpc.Core.Base;
using EasyRpc.Core.Client;
using EasyRpc.Master;
using EasyRpc.Plugin.SignalR.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRpc.Plugin.SignalR
{
    public class SigrPeerClientFactory : IPeerClientFactory
    {
        private readonly ISigrPeerClientStore _sigrPeerClientStore;

        public SigrPeerClientFactory(IServiceProvider serviceProvider)
        {
            _sigrPeerClientStore = serviceProvider.GetService<ISigrPeerClientStore>() ?? throw new TypeInitializationException($"{nameof(ISigrPeerClientStore)} not initialized", null);
        }

        public IPeerService GetHandle(RegistrationRequest registerationRequest)
        {
            var client = _sigrPeerClientStore.GetClient(registerationRequest.Properties[CommonConstants.SigrReferenceTag]);
            return client;
        }
    }
}
