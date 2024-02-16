using CommServices.CommMaster;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;

namespace GrpcNetPeer
{
    internal class PeerNetClient : IMainPeerClient
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

        public async Task<Message> MakeRequest(Message message)
        {
            message.From = _id;
            return await _client.MakeRequestAsync(message).ConfigureAwait(false);
            //return await _client.MakeRequestAsync(new Message { From = _id, To = "Master", Data = "" }).ConfigureAwait(false);
        }

        public async Task<Empty> Notify(Message message)
        {
            message.From = _id;
            return await _client.NotifyAsync(message).ConfigureAwait(false);
            //return await _client.NotifyAsync(new Message { From = _id, To = "Master", Data = "" }).ConfigureAwait(false);
        }

        public async Task<RegisterationResponse> Register(RegisterationRequest request)
        {
            var result = await _master.RegisterAsync(request);
            _id = result.RegistrationId;
            return result;
        }

        public async Task<RegisterationResponse> Unregister(RegisterationRequest request)
        {
            _id = null;
            request.RegistrationId = _id;
            return await _master.UnregisterAsync(request);            
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
