using EasyRpc.Core.Client;
using EasyRpc.Core.Util;
using EasyRpc.Master;
using EasyRpc.Types;
using Grpc.Net.Client;

namespace EasyRpc.Peer.Net
{
    internal class EasyRpcMasterClient : IMasterService
    {
        private readonly MasterService.MasterServiceClient _masterConnection;
        private readonly GrpcChannel _mChannel;
        private string? _id;

        public EasyRpcMasterClient(string masterAddress)
        {
            var serverCertProvider = new DefaultServerCertificateProvider();
            var clientCertProvider = new DefaultClientCertificateProvider();
            HttpClientHandler handler = new HttpClientHandler();
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, serverCertProvider);
            GrpcChannelSecurityHelper.SetClientCertificates(handler, clientCertProvider);

            _mChannel = GrpcChannel.ForAddress(masterAddress, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });

            _masterConnection = new MasterService.MasterServiceClient(_mChannel);
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var result = await _masterConnection.RegisterAsync(request);
            _id = result.RegistrationId;
            return result;
        }

        public async Task<RegistrationResponse> Unregister(RegistrationRequest request)
        {
            _id = null;
            request.RegistrationId = _id;
            return await _masterConnection.UnregisterAsync(request);
        }

        public async Task<Empty> Notify(Message message)
        {
            message.From = _id;
            return await _masterConnection.NotifyAsync(message).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _mChannel.ShutdownAsync().Wait();
        }
    }
}
