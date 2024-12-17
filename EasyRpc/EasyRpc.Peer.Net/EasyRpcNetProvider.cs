using EasyRpc.Core.Base;
using EasyRpc.Core.Util;
using EasyRpc.Master;
using Grpc.Core;

namespace EasyRpc.Peer.Net
{
    //TODO: Convert it to a builder pattern
    public class EasyRpcNetProvider
    {
        private Server? _server;
        private IMasterService? _masterHandle;
        private string? _registrationId { get; set; }

        public IMasterService Handle => _masterHandle!;

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
            _masterHandle = new EasyRpcMasterClient(masterAddress.OriginalString);
            var response = await _masterHandle.Register(new RegistrationRequest
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
            Task task1;
            if (_masterHandle!.IsConnected)
                task1 = _masterHandle.Unregister(new RegistrationRequest() { RegistrationId = _registrationId, Name = "Peer1", Type = "Grpc" });
            else
                task1 = Task.CompletedTask;
            var task2 = _server!.ShutdownAsync();
            Task.WaitAll([task1, task2!]);
        }
    }
}
