using CommMaster.ClientManagement;
using CommServices.CommMaster;
using CommServices.CommPeer;
using Grpc.Core;

namespace CommMaster
{
    public class CommService
    {
        private readonly IPeerRegistry _clientRegistry;
        private ManualResetEvent resetEvent = new ManualResetEvent(false);
        private Task? _masterTask;
        private Task? _peerTask;

        public CommService()
        {
            _clientRegistry = new PeerRegistry();
        }

        public void Start()
        {
            _masterTask = StartMasterAsync();
            _peerTask = StartPeerAsync();
        }

        public void Stop()
        {
            resetEvent.Set();
            Task.WaitAll(_masterTask!, _peerTask!);
        }

        public async Task StartMasterAsync()
        {
            var resolver = new PeerHandlerResolver();
            Server server = new Server
            {
                Services = { CommMasterService.BindService(new MasterService(resolver, _clientRegistry)) },
                Ports = { new ServerPort("localhost", 50051, GetSecureChannel()) }
            };

            server.Start();

            System.Diagnostics.Debug.WriteLine($"Master server listening on port {50051}");

            resetEvent.WaitOne();

            await server.ShutdownAsync();
        }

        public async Task StartPeerAsync()
        {
            Server server = new Server
            {
                Services = { CommPeerService.BindService(new PeerService(_clientRegistry)) },
                Ports = { new ServerPort("localhost", 50052, GetSecureChannel()) }
            };

            server.Start();

            System.Diagnostics.Debug.WriteLine($"Peer server listening on port {50052}");

            resetEvent.WaitOne();

            await server.ShutdownAsync();
        }

        private SslServerCredentials GetSecureChannel()
        {
            List<KeyCertificatePair> certificates = new List<KeyCertificatePair>();
            certificates.Add(new KeyCertificatePair(File.ReadAllText("C:\\certs\\CommServer.crt"), File.ReadAllText("C:\\certs\\server.key")));
            return new SslServerCredentials(certificates);
        }
    }
}
