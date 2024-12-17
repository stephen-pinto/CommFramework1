using EasyRpc.Core.Base;
using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using EasyRpc.Types;
using Microsoft.AspNetCore.SignalR;

namespace EasyRpc.Plugin.SignalR
{
    public class PeerSigrClient : IPeerService
    {
        private string _sigrConnectionId;
        private readonly IEasyRpcSignalRHub _refInterface;
        private readonly SignalRPeerHub _hubRef;
        private readonly ResponseAwaiter _responseAwaiter;
        private readonly RegistrationRequestSigr _registration;

        public RegistrationRequestSigr Registration => _registration;

        public string ConnectionId => _sigrConnectionId;

        public bool IsConnected => !string.IsNullOrEmpty(_sigrConnectionId);

        public PeerSigrClient(
            SignalRPeerHub hub,
            RegistrationRequestSigr registeration,
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

        public void Dispose()
        {
            
        }
    }
}
