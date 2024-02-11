using Grpc.Core;
using GrpcService1;

namespace CommServerLib
{
    public class CmServer
    {
        public void Start(int port)
        {
            Server server = new Server
            {
                Services = { Greeter.BindService(new GreeterService()) },
                Ports = { new ServerPort("localhost", port, GetSecureChannel()) }
            };

            server.Start();

            System.Diagnostics.Debug.WriteLine($"Greeter server listening on port {port}");

            Console.WriteLine("Press any key to stop the server");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

        private SslServerCredentials GetSecureChannel()
        {
            List<KeyCertificatePair> certificates = new List<KeyCertificatePair>();
            certificates.Add(new KeyCertificatePair(File.ReadAllText("C:\\certs\\CommServer.crt"), File.ReadAllText("C:\\certs\\server.key")));
            return new SslServerCredentials(certificates);
        }
    }
}
