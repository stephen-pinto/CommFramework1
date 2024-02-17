using CommPeerServices.Base.Client;
using CommServices.CommMaster;

namespace MasterService
{
    public class BackendClientFactory : IPeerClientFactory
    {
        private BackendClient? _instance;

        public BackendClientFactory(BackendClient client)
        {
            _instance = client;
        }

        public IPeerClient GetHandle(RegisterationRequest registerationRequest)
        {
            return _instance!;
        }
    }
}
