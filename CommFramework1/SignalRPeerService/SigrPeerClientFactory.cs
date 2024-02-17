using CommPeerServices.Base.Client;
using CommServices.CommMaster;
using Microsoft.Extensions.DependencyInjection;
using SignalRPeerService.Interfaces;

namespace SignalRPeerService
{
    public class SigrPeerClientFactory : IPeerClientFactory
    {
        private readonly ISigrPeerClientStore _sigrPeerClientStore;

        public SigrPeerClientFactory(IServiceProvider serviceProvider)
        {
            _sigrPeerClientStore = serviceProvider.GetService<ISigrPeerClientStore>() ?? throw new TypeInitializationException("ISigrPeerClientStore not initialized", null);
        }

        public IPeerClient GetHandle(RegisterationRequest registerationRequest)
        {
            var client = _sigrPeerClientStore.GetClient(registerationRequest.Properties[CommonConstants.SigrReferenceTag]);
            return client;
        }
    }
}
