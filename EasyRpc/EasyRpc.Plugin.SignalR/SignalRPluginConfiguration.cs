using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;

namespace EasyRpc.Plugin.SignalR
{
    public record SignalRPluginConfiguration : IEasyRpcPluginConfiguration
    {
        public string HostUrl { get; set; } = "https://localhost:5111";

        public string EndpointPath { get; set; } = "/peer";

        public IMasterClient? MasterClient { get; set; }

        public IPeerClient? MainPeerClient { get; set; }
    }
}
