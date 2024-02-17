using CommPeerServices.Base.Client;
using CommServices.CommShared;
using Microsoft.AspNetCore.SignalR;
using SignalRPeerService.Interfaces;
using SignalRPeerService.Types;

namespace SignalRPeerService
{
    public class SigrPeerClient : IPeerClient
    {
        private string _sigrConnectionId;
        private readonly ICommSignalRHub _refInterface;
        private readonly PeerHub _hubRef;        
        private readonly ResponseAwaiter _responseAwaiter;
        private readonly RegisterationRequestSigr _registration;

        public RegisterationRequestSigr Registration => _registration;

        public string ConnectionId => _sigrConnectionId;

        public SigrPeerClient(
            PeerHub hub, 
            RegisterationRequestSigr registeration, 
            string sigrConnectionId, 
            ResponseAwaiter responseAwaiter)
        {
            _registration = registeration;
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
