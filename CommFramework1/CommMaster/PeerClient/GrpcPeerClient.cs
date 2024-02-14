using CommPeerServices.Base.Client;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Net.Client;

namespace CommMaster.PeerClient
{
    public class GrpcPeerClient : IPeerClient
    {
        private readonly CommPeerService.CommPeerServiceClient _client;

        public GrpcPeerClient(string address, HttpClientHandler handler)
        {
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });

            _client = new CommPeerService.CommPeerServiceClient(channel);
        }

        public GrpcPeerClient(string address, GrpcChannelOptions channelOptions)
        {
            var channel = GrpcChannel.ForAddress(address, channelOptions);
            _client = new CommPeerService.CommPeerServiceClient(channel);
        }

        public async Task<Message> MakeRequest(Message message)
        {
            return await _client.MakeRequestAsync(message);
        }

        public async Task<Message> Notify(Message message)
        {
            return await _client.MakeRequestAsync(message);
        }
    }
}
