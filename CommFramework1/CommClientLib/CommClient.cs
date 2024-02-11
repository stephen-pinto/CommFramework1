using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient1;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using System.Xml.Serialization;

namespace CommClientLib
{
    public class CommClient
    {
        public void Connect(int port, string message)
        {
            Thread.Sleep(5000);

            try
            {
                var address = string.Format("https://localhost:{0}", port);
                var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
                {
                    HttpClient = GetConfiguredClient(),
                });

                var client = new Greeter.GreeterClient(channel);

                var reply = client.SayHelloAsync(
                                     new HelloRequest { Name = $"{message}" }).GetAwaiter().GetResult();

                Console.WriteLine("Greeting: " + reply.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private HttpClient GetConfiguredClient()
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
            return new HttpClient(httpClientHandler);
        }
    }
}
