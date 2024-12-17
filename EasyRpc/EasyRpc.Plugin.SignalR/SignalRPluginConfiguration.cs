using EasyRpc.Core.Base;
using EasyRpc.Core.Plugin;

namespace EasyRpc.Plugin.SignalR
{
    public delegate Tuple<IMasterService, IPeerService> MasterServiceProviderFactoryFunc();

    public record SignalRPluginConfiguration : IEasyRpcPluginConfiguration
    {
        public string HostUrl { get; set; } = "https://localhost:5111";

        public string EndpointPath { get; set; } = "/peer";

        public MasterServiceProviderFactoryFunc? MasterServiceProvider { get; set; }

        public IMasterService? MasterClient { get; set; }

        public IPeerService? MainPeerClient { get; set; }
    }
}
