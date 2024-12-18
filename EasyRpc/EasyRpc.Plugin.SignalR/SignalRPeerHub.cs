using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRpc.Plugin.SignalR
{
    public delegate Task RegisterHandler(string connectionId, RegistrationRequestSigr request);
    public delegate Task UnregisterHandler(string connectionId, RegistrationRequestSigr request);
    public delegate Task NotifyHandler(string connectionId, MessageSigr message);

    public class SignalRPeerHub : Hub, IEasyRpcSignalRHub
    {
        private ResponseAwaiter? _responseAwaiter;
        private RegisterHandler? _registerHandler;
        private UnregisterHandler? _unregisterHandler;
        private NotifyHandler? _notifyHandler;
        private readonly IServiceProvider _serviceProvider;

        public SignalRPeerHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _responseAwaiter = _serviceProvider.GetService<ResponseAwaiter>();
        }

        public void SetupHandlers(
            RegisterHandler registerHandler,
            UnregisterHandler unregisterHandler,
            NotifyHandler notifyHandler)
        {
            _registerHandler = registerHandler;
            _unregisterHandler = unregisterHandler;
            _notifyHandler = notifyHandler;
        }

        public async Task Register(RegistrationRequestSigr request)
        {
            _serviceProvider.GetRequiredService<PeerSigrBridge>();
            Console.WriteLine($"Registering {Context.ConnectionId}");
            await _registerHandler!(Context.ConnectionId, request);
        }

        public async Task Unregister(RegistrationRequestSigr request)
        {
            _serviceProvider.GetRequiredService<PeerSigrBridge>();
            Console.WriteLine($"Unregistering {Context.ConnectionId}");
            await _unregisterHandler!(Context.ConnectionId, request);
        }

        public async Task MakeRequest(MessageSigr message)
        {
            await Task.FromException(new NotSupportedException());
        }

        public async Task SendMakeRequestResponse(MessageSigr message)
        {
            Console.WriteLine($"Sending response {message.Id}");
            await Task.FromResult(() => _responseAwaiter!.SaveResponse(message.Id, message));
        }

        public async Task Notify(MessageSigr message)
        {
            _serviceProvider.GetRequiredService<PeerSigrBridge>();
            Console.WriteLine($"Notifying {message.Id}");
            await _notifyHandler!(Context.ConnectionId, message);
        }

        public Task SendRegisterResponse(RegistrationResponseSigr response)
        {
            //This is only supported by the server so clients may not call this method
            throw new NotSupportedException();
        }

        public Task SendUnregisterResponse(RegistrationResponseSigr response)
        {
            //This is only supported by the server so clients may not call this method
            throw new NotSupportedException();
        }
    }
}
