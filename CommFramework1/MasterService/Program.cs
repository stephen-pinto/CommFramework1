using CommMaster;
using CommMaster.PeerManagement;
using CommPeerServices.Base.Client;
using CommServices.CommShared;

namespace MasterService
{
    class Program
    {
        static void Main(string[] args)
        {
            TempClient tempClient = new TempClient();
            DefaultPeerClientResolver resolver = new DefaultPeerClientResolver();
            resolver.Add("TempClient", new TempClientFactory(tempClient));

            CommService service = new CommService("localhost", 50051, resolver);
            IMasterClient masterClient = service;
            IPeerClient peerClient = service;

            service.Start();

            var info = masterClient.Register(new CommServices.CommMaster.RegisterationRequest
            {
                Name = "TempClient",
                Type = "TempClient",
                Address = string.Empty,
                Properties = { { "key", "value" } },
                RegistrationId = string.Empty
            }).GetAwaiter().GetResult();

            peerClient.MakeRequest(new CommServices.CommShared.Message
            {
                From = "TempClient",
                To = info.RegistrationId,
                Data = "Hello from MasterService!"
            });

            Console.WriteLine("Press any key to stop the service...");
            Console.ReadKey();

            service.Stop();

            //SignalRPeerService.SignalRService service = new SignalRPeerService.SignalRService();
            //service.Start();
        }
    }
}

class TempClientFactory : IPeerClientFactory
{
    private TempClient _client;

    public TempClientFactory(TempClient client)
    {
        _client = client;
    }

    public IPeerClient GetHandle(CommServices.CommMaster.RegisterationRequest registerationRequest)
    {
        return _client;
    }
}

class TempClient : IPeerClient
{
    public Task<Message> MakeRequest(Message message)
    {
        Console.WriteLine($"[TempClient] Received message from {message.From} with content: {message.Data}");
        return Task.FromResult(new Message
        {
            From = "TempClient",
            To = message.From,
            Data = "Hello from TempClient!"
        });
    }

    public Task<Empty> Notify(Message message)
    {
        Console.WriteLine($"[TempClient] Received message from {message.From} with content: {message.Data}");
        return Task.FromResult(new Empty());
    }
}