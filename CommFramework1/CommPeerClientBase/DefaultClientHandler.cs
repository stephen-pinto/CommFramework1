using CommServices.CommPeer;

namespace CommPeerClientBase
{
    internal class DefaultClientHandler : IClientHandler
    {
        private CommPeerService.CommPeerServiceClient? _client;

        public CommPeerService.CommPeerServiceClient Client
        {
            get
            {
                if (_client == null)
                    throw new InvalidOperationException($"Client not initialized calle {nameof(Start)} to connect first");
                return _client;
            }
        }

        public void Start(string address)
        {
            var channelProvider = new RpcChannelProvider();
            var channel = channelProvider.GetChannel(address);
            _client = new CommPeerService.CommPeerServiceClient(channel);
        }
    }
}
