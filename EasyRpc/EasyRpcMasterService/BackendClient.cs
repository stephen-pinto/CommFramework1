using EasyRpc.Core.Client;
using EasyRpc.Types;

namespace EasyRpcMasterService
{
    public class BackendClient : IPeerClient
    {
        public Task<Message> MakeRequest(Message message)
        {
            Console.WriteLine($"[BackendClient] Received message from {message.From} with content: {message.Data}");
            return Task.FromResult(new Message
            {
                From = "BackendClient",
                To = message.From,
                Data = "Hello from BackendClient!"
            });
        }

        public Task<Empty> Notify(Message message)
        {
            Console.WriteLine($"[BackendClient] Received message from {message.From} with content: {message.Data}");
            return Task.FromResult(new Empty());
        }
    }
}
