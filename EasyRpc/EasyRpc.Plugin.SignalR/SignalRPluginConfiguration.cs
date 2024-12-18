using EasyRpc.Core.Plugin;

namespace EasyRpc.Plugin.SignalR
{
    public record SignalRPluginConfiguration : IEasyRpcPluginConfiguration
    {
        public Uri HubAddress { get; set; } = new Uri("https://localhost:55155");

        public string EndpointPath { get; set; } = "/peer";

        public string CertificateFriendlyName { get; set; } = "SigR Server Authorization";
    }
}
