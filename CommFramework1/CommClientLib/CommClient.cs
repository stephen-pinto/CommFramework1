using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient1;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;

namespace CommClientLib
{
    public class CommClient
    {
        public void Connect(string message)
        {
            Thread.Sleep(5000);

            try
            {
                MethodTwo(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MethodOne(string message)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            SslCredentials secureChanel = new SslCredentials(File.ReadAllText("C:\\certs\\CommServer.crt"));
            using var channel = GrpcChannel.ForAddress("https://localhost:50051",
                new GrpcChannelOptions { Credentials = secureChanel });
            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHelloAsync(
                                     new HelloRequest { Name = $"{message}" }).GetAwaiter().GetResult();
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void MethodTwo(string message)
        {
            // Validate the server certificate with the root CA
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, _) => {
                chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                chain.ChainPolicy.CustomTrustStore.Add(new X509Certificate2("C:\\certs\\CommServer.crt"));
                return chain.Build(cert);
            };

            // Pass the client certificate so the server can authenticate the client
            var clientCert = X509Certificate2.CreateFromPemFile("C:\\certs\\CommClient.crt", "C:\\certs\\client.key");
            httpClientHandler.ClientCertificates.Add(clientCert);

            // Create a GRPC Channel
            var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress("https://localhost:50051", new GrpcChannelOptions
            {
                HttpClient = httpClient,
            });

            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHelloAsync(
                                 new HelloRequest { Name = $"{message}" }).GetAwaiter().GetResult();

            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void MethodThree(string message)
        {
            var httpClientHandler = new HttpClientHandler();
            //httpClientHandler.ClientCertificates.Add(new X509Certificate2("C:\\certs\\CommServer.crt"));
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            // Pass the client certificate so the server can authenticate the client
            var clientCert = X509Certificate2.CreateFromPemFile("C:\\certs\\CommClient.crt", "C:\\certs\\client.key");
            httpClientHandler.ClientCertificates.Add(clientCert);

            // Create a GRPC Channel
            var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress("https://localhost:50051", new GrpcChannelOptions
            {
                HttpClient = httpClient,
            });

            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHelloAsync(
                                 new HelloRequest { Name = $"{message}" }).GetAwaiter().GetResult();

            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
