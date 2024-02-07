using Grpc.Net.Client;
using GrpcClient1;

namespace CommClientLib
{
    public class CommClient
    {
        public void Connect(string message)
        {
            try
            {
                Thread.Sleep(2000);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                using var channel = GrpcChannel.ForAddress("http://localhost:50051");
                var client = new Greeter.GreeterClient(channel);
                var reply = client.SayHelloAsync(
                                     new HelloRequest { Name = $"{message}" }).GetAwaiter().GetResult();
                Console.WriteLine("Greeting: " + reply.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
