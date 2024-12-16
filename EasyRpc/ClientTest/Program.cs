using EasyRpc.Peer.Net;
using EasyRpc.Types;

internal class Program
{
    private static EasyRpcNetProvider _services;

    private static void Main(string[] args)
    {
        Program pg = new Program();
        var curPath = Directory.GetCurrentDirectory();

        Console.WriteLine("Welcome to Peer1 service! Press any key when ready");
        Console.ReadKey();

        _services = new EasyRpcNetProvider();
        _services.Start(
            new Uri("https://localhost:50051"),
            new Uri("https://localhost:50052"),
            pg.HandleRequest).Wait();

        Console.WriteLine("[PEER1]: Press any key to test message send");
        Console.ReadKey();

        _services.Handle.Notify(new Message { Data = "Notification from Peer1" });

        Console.WriteLine("[PEER1]: Press any key to stop the service...");
        Console.ReadKey();

        _services.Stop();
    }

    public Task<Message> HandleRequest(Message message)
    {
        Console.WriteLine("[PEER1]: Request from server: " + message.Data);
        var response = new Message() { From = _services.Id, To = message.To, Type = message.Type };
        response.Metadata.Add(message.Metadata);
        response.Headers.Add(message.Headers);
        return Task.FromResult(response);
    }
}