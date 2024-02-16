using Microsoft.AspNetCore.SignalR;

namespace SignalRPeerService
{
    public class PeerHub : Hub, IPeerClientSigr, ICommMasterClientSigr
    {
        private readonly RegistererDelegate _registerHandler;
        private readonly RegistererDelegate _unregisterHandler;
        private readonly RequestDelegate _requestHandler;
        private readonly RequestDelegate _notificationHandler;

        public PeerHub(RegistererDelegate registerHandler, RegistererDelegate unregisterHandler, RequestDelegate requestHandler, RequestDelegate notificationHandler)
        {
            _registerHandler = registerHandler;
            _unregisterHandler = unregisterHandler;
            _requestHandler = requestHandler;
            _notificationHandler = notificationHandler;
        }

        public async Task<RegisterationResponseSigr> Register(RegisterationRequestSigr request)
        {
            Console.WriteLine($"Registering {Context.ConnectionId}");
            return await _registerHandler(request);

            //return Task.FromResult(new RegisterationResponseSigr
            //{
            //    RegistrationId = Guid.NewGuid().ToString(),
            //    Status = "Success",
            //});
        }

        public async Task<RegisterationResponseSigr> Unregister(RegisterationRequestSigr request)
        {
            Console.WriteLine($"Unregistering {Context.ConnectionId}");
            return await _unregisterHandler(request);

            //return Task.FromResult(new RegisterationResponseSigr
            //{
            //    RegistrationId = Guid.NewGuid().ToString(),
            //    Status = "Success",
            //});
        }

        public async Task<MessageSigr> MakeRequest(MessageSigr message)
        {
            Console.WriteLine($"Making request {message.Id}");
            return await _requestHandler(message);
            //return Task.FromResult(new MessageSigr(null, null, null, null, null, null, null));
        }

        public async Task Notify(MessageSigr message)
        {
            Console.WriteLine($"Notifying {message.Id}");
            await _notificationHandler(message);
            //return Task.FromResult(new MessageSigr(null, null, null, null, null, null, null));
        }
    }
}