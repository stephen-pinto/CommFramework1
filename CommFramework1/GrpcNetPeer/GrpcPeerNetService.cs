using CommPeerServices.Base.Client;
using CommServices.CommMaster;
using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace GrpcNetPeer
{
    public class GrpcPeerNetService
    {
        private Server? _server;
        private IMainPeerClient? _peerClient;

        public IPeerClient PeerClient => _peerClient!;

        public async Task Start(string masterAddress, string masterPeerAddress, string myAddress)
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
                Services = { CommPeerService.BindService(new PeerService(MakeRequest, Notify)) },
                Ports = { new ServerPort("localhost", 50055, GetSecureChannel()) }
            };

            _server.Start();
        }

        public void Stop()
        {
            var task1 = _peerClient!.Unregister(new RegisterationRequest() { Name = "Peer1", Type = "Grpc" });
            var task2 = _server?.ShutdownAsync();
            Task.WaitAll([task1!, task2!]);
        }

        public async Task<Message> MakeRequest(Message message)
        {
            Console.WriteLine($"Received request from {message.From} to {message.To} with body = {message.Data}");
            return await Task.FromResult(new Message
            {
                From = "Peer",
                To = message.From,
                Data = "Successfully received request: " + message.Data
            });
        }

        public async Task<Empty> Notify(Message message)
        {
            Console.WriteLine($"Received request from {message.From} to {message.To} with body = {message.Data}");
            return await Task.FromResult(new Empty());
        }

        private SslServerCredentials GetSecureChannel()
        {
            List<KeyCertificatePair> certificates = new List<KeyCertificatePair>();
            certificates.Add(new KeyCertificatePair(File.ReadAllText("C:\\certs\\CommServer.crt"), File.ReadAllText("C:\\certs\\server.key")));
            return new SslServerCredentials(certificates);
        }
    }
}
