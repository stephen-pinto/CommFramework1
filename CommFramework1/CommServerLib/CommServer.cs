using Grpc.Core;
using GrpcService1;

namespace CommServerLib
{
    public class CommServer
    {
        public void Start()
        {
            List<KeyCertificatePair> certificados = new List<KeyCertificatePair>();
            certificados.Add(new KeyCertificatePair(File.ReadAllText("C:\\certs\\CommServer.crt"), File.ReadAllText("C:\\certs\\server.key")));
            ServerCredentials servCred = new SslServerCredentials(certificados);

            Server server = new Server
            {
                Services = { Greeter.BindService(new GreeterService()) },
                Ports = { new ServerPort("localhost", 50051, servCred) }
            };

            server.Start();

            Console.WriteLine("Greeter server listening on port 50051");
            Console.WriteLine("Press any key to stop the server");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
