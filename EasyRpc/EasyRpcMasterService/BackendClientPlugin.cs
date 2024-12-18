using EasyRpc.Core.Base;
using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Master;

namespace EasyRpcMasterService
{
    internal class BackendPluginConfiguration : IEasyRpcPluginConfiguration
    {
    }

    internal class BackendClientPlugin : IEasyRpcPlugin
    {
        private BackendClient? _backendClient;
        private BackendPluginConfiguration? _configuration;
        private IMasterService? _masterService;

        public string TypeIdentifier => "BackendClient";

        public IPeerClientFactory GetClientFactory()
        {
            if (_backendClient == null)
                throw new ArgumentNullException("Not initialized. Call " + nameof(Init) + " first.");

            return new BackendClientFactory(_backendClient);
        }

        public void Init(IEasyRpcPluginConfiguration config)
        {
            _configuration = config as BackendPluginConfiguration;
            _backendClient = new BackendClient();
        }

        public void Test()
        {
            var info = _masterService?.Register(new RegistrationRequest
            {
                Name = "BackendClient",
                Type = "BackendClient",
                Address = string.Empty,
                Properties = { { "key", "value" } },
                RegistrationId = string.Empty
            }).GetAwaiter().GetResult();
        }

        public void Load(IMasterService masterService)
        {
            _masterService = masterService;
        }

        public void Unload() { }
    }
}
