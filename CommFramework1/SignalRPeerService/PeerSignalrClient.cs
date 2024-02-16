using CommPeerServices.Base.Client;
using CommPeerServices.Base.Server;
using CommServices.CommShared;
using Microsoft.AspNetCore.SignalR;

namespace SignalRPeerService
{
    public class PeerSignalrClient : IPeerClient
    {
        private Hub? _hubRef;
        private string _connectionId;
        private IPeerService _peerServer;

        public PeerSignalrClient(IPeerService peerServer, Hub hub, string connectionId)
        {
            _hubRef = hub;
            _connectionId = connectionId;
            _peerServer = peerServer;
        }

        public Task<Message> MakeRequest(Message message)
        {
            //TODO: await strategy
            MessageSigr msg = message;
            _hubRef?.Clients.Client(_connectionId).SendAsync("MakeRequest", msg);
            throw new NotImplementedException();
        }

        public Task<Empty> Notify(Message message)
        {
            //TODO: await strategy
            MessageSigr msg = message;
            _hubRef?.Clients.Client(_connectionId).SendAsync("Notify", msg);
            throw new NotImplementedException();
        }
    }
}
