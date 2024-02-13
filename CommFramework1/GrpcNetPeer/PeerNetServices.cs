using CommServices.CommPeer;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using System.Net;

namespace GrpcNetPeer
{
    public class PeerNetServices
    {
        private PeerNetClient? _peerClient;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);

        public async Task Start(string masterAddress, string masterPeerAddress, string myAddress)
        {
            _peerClient = new PeerNetClient(masterAddress, masterPeerAddress);
            _peerClient.Register(myAddress);

            Server server = new Server
            {
                Services = { CommPeerService.BindService(new PeerService()) },
                Ports = { new ServerPort("localhost", 50055, GetSecureChannel()) }
            };

            server.Start();

            await Task.Factory.StartNew(() =>
            {
                _resetEvent.WaitOne();
                server.ShutdownAsync().Wait();
            });
        }

        public void Stop()
        {
            _resetEvent.Set();
            _peerClient!.UnRegister();
        }

        public void MakeRequest(string message)
        {
            _peerClient!.MakeRequest(message);
        }

        public void Notify(string message)
        {
            _peerClient!.Notify(message);
        }

        private SslServerCredentials GetSecureChannel()
        {
            List<KeyCertificatePair> certificates = new List<KeyCertificatePair>();
            certificates.Add(new KeyCertificatePair(File.ReadAllText("C:\\certs\\CommServer.crt"), File.ReadAllText("C:\\certs\\server.key")));
            return new SslServerCredentials(certificates);
        }
    }
}
