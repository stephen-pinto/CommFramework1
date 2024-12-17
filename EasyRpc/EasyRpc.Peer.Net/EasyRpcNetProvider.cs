using EasyRpc.Core.Client;
using EasyRpc.Core.Util;
using EasyRpc.Master;
using Grpc.Core;

namespace EasyRpc.Peer.Net
{
    //TODO: Convert it to a builder pattern
    public class EasyRpcNetProvider
    {
        private Server? _server;
        private IMasterService? _masterClient;
        private string? _registrationId { get; set; }

        public IMasterService Handle => _masterClient!;

        public string Id => _registrationId!;

        public async Task Start(
            Uri masterAddress,
            Uri myAddress,
            MakeRequestDelegate makeRequestHandler)
        {
            ICertificateProvider certificateProvider = new DefaultClientCertificateProvider();
            
            _server = new Server
            {
                Services = { PeerService.BindService(new PeerNetService(makeRequestHandler)) },
                Ports = { new ServerPort(myAddress.Host, myAddress.Port, GrpcChannelSecurityHelper.GetSecureServerCredentials(certificateProvider)) }
            };
            _server.Start();

            //TODO: Move the masterPeerAddress to fetch from registration response
            _masterClient = new EasyRpcMasterClient(masterAddress.OriginalString);
            var response = await _masterClient.Register(new RegistrationRequest
            {
                Address = myAddress.OriginalString,
                Name = "Peer1",
                Type = "Grpc",
                Properties = { { "OS", "Windows" }, { "Version", "10" } },
            }).ConfigureAwait(false);

            _registrationId = response.RegistrationId;
        }

        public void Stop()
        {
            var task1 = _masterClient!.Unregister(new RegistrationRequest() { RegistrationId = _registrationId, Name = "Peer1", Type = "Grpc" });
            var task2 = _server!.ShutdownAsync();
            Task.WaitAll([task1!, task2!]);
        }
    }
}
