using EasyRpc.Core.Client;
using EasyRpc.Core.Util;
using EasyRpc.Master;
using EasyRpc.Types;
using Grpc.Net.Client;

namespace EasyRpc.Peer.Net
{
    internal class PeerNetClient : IEasyRpcServicesClient
    {
        private readonly PeerService.PeerServiceClient _client;
        private readonly MasterService.MasterServiceClient _master;
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
            _client = new PeerService.PeerServiceClient(pchannel);

            var mchannel = GrpcChannel.ForAddress(masterAddress, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });
            _master = new MasterService.MasterServiceClient(mchannel);
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

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var result = await _master.RegisterAsync(request);
            _id = result.RegistrationId;
            return result;
        }

        public async Task<RegistrationResponse> Unregister(RegistrationRequest request)
        {
            _id = null;
            request.RegistrationId = _id;
            return await _master.UnregisterAsync(request);
        }
    }
}
