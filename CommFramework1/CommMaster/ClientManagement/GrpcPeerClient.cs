using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;

namespace CommMaster.ClientManagement
{
    public class GrpcPeerClient : IPeerClient
    {
        private readonly CommPeerService.CommPeerServiceClient _client;

        public GrpcPeerClient(string address)
        {
            var pchannel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                HttpClient = GetConfiguredClient()
            });

            _client = new CommPeerService.CommPeerServiceClient(pchannel);
        }

        public async Task<Message> MakeRequest(Message message)
        {
            return await _client.MakeRequestAsync(message);
        }

        public async Task<Message> Notify(Message message)
        {
            return await _client.MakeRequestAsync(message);
        }

        private HttpClient GetConfiguredClient()
        {
            //TODO: Make this configurable and injectable
            // Validate the server certificate with the root CA
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, _) =>
            {
                chain!.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                chain.ChainPolicy.CustomTrustStore.Add(new X509Certificate2("C:\\certs\\CommServer.crt"));
                return chain.Build(cert!);
            };

            // Pass the client certificate so the server can authenticate the client
            var clientCert = X509Certificate2.CreateFromPemFile("C:\\certs\\CommClient.crt", "C:\\certs\\client.key");
            httpClientHandler.ClientCertificates.Add(clientCert);

            // Create a GRPC Channel
            return new HttpClient(httpClientHandler);
        }
    }
}
