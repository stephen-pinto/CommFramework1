using EasyRpc.Core.Client;

namespace EasyRpc.Core.Plugin
{
    public interface IEasyRpcPluginConfiguration
    {
        public IMasterClient? MasterClient { get; set; }

        public IPeerClient? MainPeerClient { get; set; }
    }
}
