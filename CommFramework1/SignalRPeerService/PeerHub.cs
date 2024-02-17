using Microsoft.AspNetCore.SignalR;
using SignalRPeerService.Old;

namespace SignalRPeerService
{
    public class PeerHub : Hub, ICommSignalRHub
    {
        private readonly SigrClientRegister _clientRegister;
        private readonly ResponseAwaiter _responseAwaiter;

        public PeerHub(ResponseAwaiter responseAwaiter)
        {
            _responseAwaiter = responseAwaiter;
            _clientRegister = new SigrClientRegister();
        }

        public async Task Register(RegisterationRequestSigr request)
        {
            Console.WriteLine($"Registering {Context.ConnectionId}");

            var proxyClient = _clientRegister.GetProxyClientForConnection(this, Context.ConnectionId);

            //Steps:
            //1. Add the client to the register
            //2. Send registration request to the comm master
            //3. Send registration response to the client

            //return Task.FromResult(new RegisterationResponseSigr
            //{
            //    RegistrationId = Guid.NewGuid().ToString(),
            //    Status = "Success",
            //});
        }

        public Task SendRegisterResponse(RegisterationResponseSigr response)
        {
            //This is only supported by the server so clients may not call this method
            throw new NotSupportedException();
        }

        public async Task Unregister(RegisterationRequestSigr request)
        {
            Console.WriteLine($"Unregistering {Context.ConnectionId}");

            //return Task.FromResult(new RegisterationResponseSigr
            //{
            //    RegistrationId = Guid.NewGuid().ToString(),
            //    Status = "Success",
            //});
        }

        public Task SendUnregisterResponse(RegisterationResponseSigr response)
        {
            //This is only supported by the server so clients may not call this method
            throw new NotSupportedException();
        }

        public async Task MakeRequest(MessageSigr message)
        {
            Console.WriteLine($"Making request {message.Id}");
            //return Task.FromResult(new MessageSigr(null, null, null, null, null, null, null));
        }

        public async Task SendMakeRequestResponse(MessageSigr message)
        {
            Console.WriteLine($"Sending response {message.Id}");
            _responseAwaiter.SaveResponse(message.Id, message);
        }

        public async Task Notify(MessageSigr message)
        {
            Console.WriteLine($"Notifying {message.Id}");
            //return Task.FromResult(new MessageSigr(null, null, null, null, null, null, null));
        }
    }
}