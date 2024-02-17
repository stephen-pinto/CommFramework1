using CommPeerServices.Base.Client;
using CommServices.CommMaster;
using SignalRPeerService.Interfaces;

namespace SignalRPeerService
{
    public class SigrPeerClientFactory : IPeerClientFactory
    {
        private readonly ISigrPeerClientFactory _sigrPeerClientFactory;

        public SigrPeerClientFactory(ISigrPeerClientFactory sigrPeerClientFactory)
        {
            _sigrPeerClientFactory = sigrPeerClientFactory;
        }

        public IPeerClient GetHandle(RegisterationRequest registerationRequest)
        {
            var client = _sigrPeerClientFactory.GetClient(registerationRequest.Properties[CommonConstants.SigrReferenceTag]);
            return client;
        }
    }
}
