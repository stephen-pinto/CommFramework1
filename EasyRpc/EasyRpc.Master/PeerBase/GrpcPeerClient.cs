using EasyRpc.Core.Base;
using EasyRpc.Peer;
using EasyRpc.Types;
using Grpc.Net.Client;

namespace EasyRpc.Master.PeerBase
{
    public class GrpcPeerClient : IPeerService
    {
        private readonly PeerService.PeerServiceClient _client;
        private readonly GrpcChannel _channel;

        public bool IsConnected => throw new NotImplementedException();

        public GrpcPeerClient(string address, HttpClientHandler handler)
        {
            _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });

            _client = new PeerService.PeerServiceClient(_channel);
        }

        public GrpcPeerClient(string address, GrpcChannelOptions channelOptions)
        {
            var channel = GrpcChannel.ForAddress(address, channelOptions);
            _client = new PeerService.PeerServiceClient(channel);
        }

        public async Task<Message> MakeRequest(Message message)
        {
            return await _client.MakeRequestAsync(message);
        }

        public void Dispose()
        {
            _channel.ShutdownAsync().GetAwaiter().GetResult();
        }
    }
}
