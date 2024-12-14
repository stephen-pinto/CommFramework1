using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Master;
using EasyRpc.Types;

namespace EasyRpcMasterService
{
    internal class BackendPluginConfiguration : IEasyRpcPluginConfiguration
    {
        public IMasterService? MasterClient { get; set; }

        public IPeerService? MainPeerClient { get; set; }
    }

    internal class BackendClientPlugin : IEasyRpcPlugin
    {
        private BackendClient? _backendClient;
        private BackendPluginConfiguration? _configuration;

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
            var info = _configuration?.MasterClient?.Register(new RegistrationRequest
            {
                Name = "BackendClient",
                Type = "BackendClient",
                Address = string.Empty,
                Properties = { { "key", "value" } },
                RegistrationId = string.Empty
            }).GetAwaiter().GetResult();

            _configuration?.MainPeerClient?.MakeRequest(new Message
            {
                From = "BackendClient",
                To = info?.RegistrationId,
                Data = "Hello from MasterService!"
            });
        }

        public void Load() { }

        public void Unload() { }
    }
}
