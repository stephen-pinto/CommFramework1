using CommPeerServices.Base.Client;
using CommServices.CommShared;
using Microsoft.AspNetCore.SignalR;

namespace SignalRPeerService
{
    public class SigrPeerClient : IPeerClient
    {
        private readonly Hub _hubRef;
        private string _sigrConnectionId;
        private ResponseAwaiter _responseAwaiter;

        public SigrPeerClient(Hub hub, string sigrConnectionId, ResponseAwaiter responseAwaiter)
        {
            _hubRef = hub;
            _sigrConnectionId = sigrConnectionId;
            _responseAwaiter = responseAwaiter;
        }

        public Task<Message> MakeRequest(Message message)
        {
            MessageSigr messageSigr = message;
            _hubRef.Clients.Client(_sigrConnectionId).SendAsync("MakeRequest", messageSigr);
            _responseAwaiter.AwaitResponse(message.Id, new ManualResetEventSlim(false));
        }

        public Task<Empty> Notify(Message message)
        {
            MessageSigr messageSigr = message;
            _hubRef.Clients.Client(_sigrConnectionId).SendAsync("Notify", messageSigr);
        }
    }
}
