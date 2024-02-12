using CommClient;

namespace CommClientBase
{
    internal class ClientHandler : IClientHandler
    {
        private CommService.CommServiceClient? _client;

        public CommService.CommServiceClient Client
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
            _client = new CommService.CommServiceClient(channel);
        }
    }
}
