using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;

namespace CommClientBase
{
    internal class RpcChannelProvider
    {
        public GrpcChannel GetChannel(string address)
        {
            return GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                HttpClient = GetConfiguredHttpClient(),
            });
        }

        private HttpClient GetConfiguredHttpClient()
        {
            // Validate the server certificate with the root CA
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, _) =>
            {
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
