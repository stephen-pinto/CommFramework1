using EasyRpc.Core.Client;
using EasyRpc.Master;

namespace EasyRpcMasterService
{
    public class BackendClientFactory : IPeerClientFactory
    {
        private BackendClient? _instance;

        public BackendClientFactory(BackendClient client)
        {
            _instance = client;
        }
        
        public IPeerClient GetHandle(RegistrationRequest registerationRequest)
        {
            return _instance!;
        }
    }
}
