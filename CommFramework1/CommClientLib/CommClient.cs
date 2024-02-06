using Grpc.Net.Client;
using GrpcClient1;

namespace CommClientLib
{
    public class CommClient
    {
        public async void Connect(string message)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:50051");
                var client = new Greeter.GreeterClient(channel);
                var reply = await client.SayHelloAsync(
                                     new HelloRequest { Name = $"{message}" });
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
