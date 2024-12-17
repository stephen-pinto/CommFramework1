using EasyRpc.Core.Base;
using EasyRpc.Core.Util;
using EasyRpc.Master;
using EasyRpc.Types;
using Grpc.Net.Client;

namespace EasyRpc.Peer.Net
{
    internal class EasyRpcMasterClient : IMasterService
    {
        private readonly MasterService.MasterServiceClient _masterConnection;
        private readonly GrpcChannel _channel;
        private string? _id;
        private bool _disposed;

        public bool IsConnected => !_disposed;

        public EasyRpcMasterClient(string masterAddress)
        {
            var serverCertProvider = new DefaultServerCertificateProvider();
            var clientCertProvider = new DefaultClientCertificateProvider();
            HttpClientHandler handler = new HttpClientHandler();
            GrpcChannelSecurityHelper.SetAutoTrustedServerCertificates(handler, serverCertProvider);
            GrpcChannelSecurityHelper.SetClientCertificates(handler, clientCertProvider);

            _channel = GrpcChannel.ForAddress(masterAddress, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });

            _masterConnection = new MasterService.MasterServiceClient(_channel);
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
            _channel.ShutdownAsync().Wait();
            _disposed = true;
        }
    }
}
