using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRpc.Plugin.SignalR
{
    public delegate Task RegisterHandler(string connectionId, RegistrationRequestSigr request);
    public delegate Task UnregisterHandler(string connectionId, RegistrationRequestSigr request);
    public delegate Task MakeRequestHandler(string connectionId, MessageSigr message);
    public delegate Task NotifyHandler(string connectionId, MessageSigr message);

    public class PeerHub : Hub, IEasyRpcSignalRHub
    {
        private ResponseAwaiter? _responseAwaiter;
        private RegisterHandler? _registerHandler;
        private MakeRequestHandler? _makeRequestHandler;
        private UnregisterHandler? _unregisterHandler;
        private NotifyHandler? _notifyHandler;

        public PeerHub(IServiceProvider serviceProvider)
        {
            _responseAwaiter = serviceProvider.GetService<ResponseAwaiter>();
        }

        public void SetupHandlers(
            RegisterHandler registerHandler,
            UnregisterHandler unregisterHandler,
            MakeRequestHandler makeRequestHandler,
            NotifyHandler notifyHandler)
        {
            _registerHandler = registerHandler;
            _makeRequestHandler = makeRequestHandler;
            _unregisterHandler = unregisterHandler;
            _notifyHandler = notifyHandler;
        }

        public async Task Register(RegistrationRequestSigr request)
        {
            Console.WriteLine($"Registering {Context.ConnectionId}");
            await _registerHandler!(Context.ConnectionId, request);
        }

        public async Task Unregister(RegistrationRequestSigr request)
        {
            Console.WriteLine($"Unregistering {Context.ConnectionId}");
            await _unregisterHandler!(Context.ConnectionId, request);
        }

        public async Task MakeRequest(MessageSigr message)
        {
            Console.WriteLine($"Making request {message.Id}");
            await _makeRequestHandler!(Context.ConnectionId, message);
        }

        public async Task SendMakeRequestResponse(MessageSigr message)
        {
            Console.WriteLine($"Sending response {message.Id}");
            await Task.FromResult(() => _responseAwaiter!.SaveResponse(message.Id, message));
        }

        public async Task Notify(MessageSigr message)
        {
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
