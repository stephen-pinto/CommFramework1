using Microsoft.AspNetCore.SignalR;

namespace SignalRPeerService
{
    public class PeerHub : Hub, IPeerClientSigr, ICommMasterClientSigr
    {
        public Task<RegisterationResponseSigr> Register(RegisterationRequestSigr request)
        {
            Console.WriteLine($"Registering {Context.ConnectionId}");
            return Task.FromResult(new RegisterationResponseSigr
            {
                RegistrationId = Guid.NewGuid().ToString(),
                Status = "Success",
            });
        }

        public Task<RegisterationResponseSigr> Unregister(RegisterationRequestSigr request)
        {
            Console.WriteLine($"Unregistering {Context.ConnectionId}");
            return Task.FromResult(new RegisterationResponseSigr
            {
                RegistrationId = Guid.NewGuid().ToString(),
                Status = "Success",
            });
        }

        public Task<MessageSigr> MakeRequest(MessageSigr message)
        {
            Console.WriteLine($"Making request {message.Id}");
            return Task.FromResult(new MessageSigr(null, null, null, null, null, null, null));
        }

        public Task<MessageSigr> Notify(MessageSigr message)
        {
            Console.WriteLine($"Notifying {message.Id}");
            return Task.FromResult(new MessageSigr(null, null, null, null, null, null, null));
        }
    }
}