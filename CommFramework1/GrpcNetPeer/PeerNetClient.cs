using CommServices.CommMaster;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;

namespace GrpcNetPeer
{
    internal class PeerNetClient
    {
        private readonly CommPeerService.CommPeerServiceClient _client;
        private readonly CommMasterService.CommMasterServiceClient _master;
        private string? _id;

        public PeerNetClient(
            string masterAddress,
            string masterClient)
        {
            var pchannel = GrpcChannel.ForAddress(masterClient, new GrpcChannelOptions
            {
                HttpClient = GetConfiguredClient()
            });
            _client = new CommPeerService.CommPeerServiceClient(pchannel);

            var mchannel = GrpcChannel.ForAddress(masterAddress, new GrpcChannelOptions
            {
                HttpClient = GetConfiguredClient()
            });
            _master = new CommMasterService.CommMasterServiceClient(mchannel);
        }

        public void Register(string peerServiceAddress)
        {
            var result = _master.Register(new RegisterationRequest
            {
                Address = peerServiceAddress,
                Name = "Peer1",
                Type = "Grpc",
                Properties = { { "OS", "Windows" }, { "Version", "10" } },
            });

            _id = result.RegistrationId;
        }

        public void UnRegister()
        {
            _master.Unregister(new RegisterationRequest
            {
                RegistrationId = _id
            });
        }

        public void MakeRequest(string message)
        {
            _client.MakeRequest(new Message { From = "Peer", To = "Master", Data = message });
        }

        public void Notify(string message)
        {
            _client.MakeRequest(new Message { From = "Peer", To = "Master", Data = message });
        }

        private HttpClient GetConfiguredClient()
        {
            // Validate the server certificate with the root CA
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, _) =>
            {
                chain!.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
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
