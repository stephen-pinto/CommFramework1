using EasyRpc.Core.Base;
using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using EasyRpc.Types;
using Microsoft.AspNetCore.SignalR;

namespace EasyRpc.Plugin.SignalR
{
    public class PeerSigrClient : IPeerService
    {
        private readonly string _sigrConnectionId;
        private readonly SignalRPeerHub _hubRef;
        private readonly ResponseAwaiter _responseAwaiter;

        public RegistrationRequestSigr? Registration { get; set; }

        public string ConnectionId => _sigrConnectionId;

        public bool IsConnected => !string.IsNullOrEmpty(_sigrConnectionId);

        public PeerSigrClient(
            SignalRPeerHub hub,
            RegistrationRequestSigr registration,
            string sigrConnectionId,
            ResponseAwaiter responseAwaiter)
        {
            _hubRef = hub;
            _sigrConnectionId = sigrConnectionId;
            _responseAwaiter = responseAwaiter;
            Registration = registration;
        }

        public async Task<Message> MakeRequest(Message message)
        {
            MessageSigr messageSigr = message;
            return await Task.Run(() =>
            {
                _hubRef.Clients.Client(_sigrConnectionId).SendAsync(nameof(IEasyRpcSignalRHub.MakeRequest), messageSigr);
                var response = _responseAwaiter.AwaitResponse(messageSigr.Id, new ManualResetEventSlim(false));
                if (response == null)
                    throw new Exception("Response not received");
                return (Message)response;
            });
        }

        public void Dispose()
        { }
    }
}
