using CommMaster.PeerClient;
using CommMaster.PeerManagement;
using CommMaster.Util;
using CommServices.CommMaster;
using CommServices.CommPeer;
using Grpc.Core;
using System.Net;
using System.Net.Sockets;

namespace CommMaster
{
    public class CommService
    {
        private readonly IPeerRegistry _clientRegistry;
        private readonly string _serviceHost;
        private readonly int _port;
        private Server? _masterServer;
        private Server? _peerServer;
        private readonly PeerClientResolver _resolver;

        private string Address => $"https://{_serviceHost}:{_port}";

        public CommService(string serviceHost, int port)
        {
            _serviceHost = serviceHost;
            _port = port;
            _clientRegistry = new PeerRegistry();
            _resolver = new PeerClientResolver
            {
                { "Grpc", new GrpcPeerClientFactory() }
            };
        }

        public void Start()
        {
            //Setup Master Server
            _masterServer = new Server
            {
                Services = { CommMasterService.BindService(new MasterService(_resolver, _clientRegistry)) },
                Ports = { new ServerPort(_serviceHost, _port, GrpcChannelSecurityHelper.GetSecureServerCredentials(CommonConstants.ServerCertificatePath, CommonConstants.ServerKeyPath)) }
            };

            _masterServer.Start();
            System.Diagnostics.Debug.WriteLine($"Master server listening on port {_port}");

            //Setup Peer Server
            _peerServer = new Server
            {
                Services = { CommPeerService.BindService(new PeerService(_clientRegistry)) },
                Ports = { new ServerPort(_serviceHost, _port + 1, GrpcChannelSecurityHelper.GetSecureServerCredentials(CommonConstants.ServerCertificatePath, CommonConstants.ServerKeyPath)) }
            };

            _peerServer.Start();
            System.Diagnostics.Debug.WriteLine($"Peer server listening on port {_port + 1}");
        }

        public void Stop()
        {
            Task.WaitAll(_masterServer!.ShutdownAsync(), _peerServer!.ShutdownAsync());
        }

        //TODO: Move this to a utility class
        private int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}
