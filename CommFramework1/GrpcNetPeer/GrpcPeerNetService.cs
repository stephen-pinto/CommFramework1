using CommPeerServices.Base.Client;
using CommPeerServices.Base.Util;
using CommServices.CommMaster;
using CommServices.CommPeer;
using Grpc.Core;

namespace GrpcNetPeer
{
    public class GrpcPeerNetService
    {
        private Server? _server;
        private IMainPeerClient? _peerClient;

        public IPeerClient PeerClient => _peerClient!;

        public async Task Start(
            string masterAddress, 
            string masterPeerAddress, 
            string myAddress,
            MakeRequestDelegate makeRequestHandler,
            NotifyDelegate notifyHandler)
        {
            //TODO: Move the masterPeerAddress to fetch from registration response
            _peerClient = new PeerNetClient(masterAddress, masterPeerAddress);
            var response = await _peerClient.Register(new RegisterationRequest
            {
                Address = masterPeerAddress,
                Name = "Peer1",
                Type = "Grpc",
                Properties = { { "OS", "Windows" }, { "Version", "10" } },
            }).ConfigureAwait(false);

            _server = new Server
            {
                Services = { CommPeerService.BindService(new PeerService(makeRequestHandler, notifyHandler)) },
                Ports = { new ServerPort("localhost", 50055, GrpcChannelSecurityHelper.GetSecureServerCredentials("C:\\certs\\CommServer.crt", "C:\\certs\\server.key")) }
            };

            _server.Start();
        }

        public void Stop()
        {
            var task1 = _peerClient!.Unregister(new RegisterationRequest() { Name = "Peer1", Type = "Grpc" });
            var task2 = _server?.ShutdownAsync();
            Task.WaitAll([task1!, task2!]);
        }
    }
}
