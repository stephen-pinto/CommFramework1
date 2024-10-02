using EasyRpc.Peer.Net;
using EasyRpc.Types;

internal class Program
{
    private static void Main(string[] args)
    {
        Program pg = new Program();
        var curPath = Directory.GetCurrentDirectory();

        Console.WriteLine("Welcome to Peer1 service!");
        PeerNetServices services = new PeerNetServices();
        services.Start(
            "https://localhost:50051",
            "https://localhost:50052",
            "https://localhost:50055",
            pg.HandleRequest, pg.HandleNotification).Wait();

        Console.WriteLine("Press any key to test message send");
        Console.ReadKey();

        services.PeerClient.MakeRequest(new Message()
        {
            Data = "Hello from Peer1"
        }).GetAwaiter().GetResult();

        Console.WriteLine("Press any key to stop the service...");
        Console.ReadKey();

        services.Stop();
    }

    public Task<Message> HandleRequest(Message message)
    {
        throw new NotImplementedException();
    }

    public Task<Empty> HandleNotification(Message message)
    {
        throw new NotImplementedException();
    }
}