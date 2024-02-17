using CommPeerServices.Base.Client;
using CommServices.CommShared;
using Microsoft.AspNetCore.SignalR;

namespace SignalRPeerService
{
    public class SigrPeerClient : IPeerClient
    {
        private string _sigrConnectionId;
        private string? _registrationId;
        private readonly ICommSignalRHub _refInterface;
        private readonly PeerHub _hubRef;        
        private readonly ResponseAwaiter _responseAwaiter;

        public SigrPeerClient(PeerHub hub, string sigrConnectionId, ResponseAwaiter responseAwaiter)
        {
            _refInterface = _hubRef = hub;
            _sigrConnectionId = sigrConnectionId;
            _responseAwaiter = responseAwaiter;
        }

        public async Task<Message> MakeRequest(Message message)
        {
            MessageSigr messageSigr = message;
            await _hubRef.Clients.Client(_sigrConnectionId).SendAsync(nameof(_refInterface.MakeRequest), messageSigr);
            var response = _responseAwaiter.AwaitResponse(messageSigr.Id, new ManualResetEventSlim(false));
            if (response == null)
                throw new Exception("Response not received");
            return (Message)response;
        }

        public async Task<Empty> Notify(Message message)
        {
            MessageSigr messageSigr = message;
            await _hubRef.Clients.Client(_sigrConnectionId).SendAsync(nameof(_refInterface.Notify), messageSigr);
            return new Empty();
        }
    }
}
