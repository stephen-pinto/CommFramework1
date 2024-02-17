using CommPeerServices.Base.Client;
using CommPeerServices.Base.Plugins;

namespace SignalRPeerService
{
    public record SignalRPluginConfiguration : IPluginConfiguration
    {
        public string HostUrl { get; set; } = "https://localhost:5111";

        public string EndpointPath { get; set; } = "/peer";

        public IMasterClient? MasterClient { get; set; }

        public IPeerClient? MainPeerClient { get; set; }
    }
}
