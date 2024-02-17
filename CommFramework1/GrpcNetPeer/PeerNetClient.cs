using CommPeerServices.Base.Util;
using CommServices.CommMaster;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Net.Client;

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
            HttpClientHandler handler = new HttpClientHandler();
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(
                handler, 
                "C:\\certs\\CommServer.crt");
            GrpcChannelSecurityHelper.SetClientCertificates(
                handler, 
                "C:\\certs\\CommClient.crt", 
                "C:\\certs\\client.key");

            var pchannel = GrpcChannel.ForAddress(masterClient, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });
            _client = new CommPeerService.CommPeerServiceClient(pchannel);

            var mchannel = GrpcChannel.ForAddress(masterAddress, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });
            _master = new CommMasterService.CommMasterServiceClient(mchannel);
        }

        public async Task<Message> MakeRequest(Message message)
        {
            message.From = _id;
            return await _client.MakeRequestAsync(message).ConfigureAwait(false);            
        }

        public async Task<Empty> Notify(Message message)
        {
            message.From = _id;
            return await _client.NotifyAsync(message).ConfigureAwait(false);            
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
    }
}
