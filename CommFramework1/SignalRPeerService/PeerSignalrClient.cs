using CommPeerServices.Base.Client;
using CommServices.CommShared;
using Microsoft.AspNetCore.SignalR;

namespace SignalRPeerService
{
    public class PeerSignalrClient : IPeerClient
    {
        private Hub? _hubRef;
        private string _connectionId;

        public PeerSignalrClient(Hub hub, string connectionId)
        {
            _hubRef = hub;
            _connectionId = connectionId;
        }

        public Task<Message> MakeRequest(Message message)
        {
            //TODO: await strategy
            MessageSigr msg = message;
            _hubRef?.Clients.Client(_connectionId).SendAsync("MakeRequest", msg);
            throw new NotImplementedException();
        }

        public Task<Message> Notify(Message message)
        {
            //TODO: await strategy
            MessageSigr msg = message;
            _hubRef?.Clients.Client(_connectionId).SendAsync("Notify", msg);
            throw new NotImplementedException();
        }
    }
}
