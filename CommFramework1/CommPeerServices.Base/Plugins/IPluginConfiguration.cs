using CommPeerServices.Base.Client;

namespace CommPeerServices.Base.Plugins
{
    public interface IPluginConfiguration
    {
        public IMasterClient? MasterClient { get; set; }

        public IPeerClient? MainPeerClient { get; set; }
    }
}
