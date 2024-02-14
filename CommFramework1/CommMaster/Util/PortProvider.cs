using System.Net.Sockets;
using System.Net;

namespace CommMaster.Util
{
    public static class PortProvider
    {
        public static int FreeTcpPort()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
            tcpListener.Stop();
            return port;
        }
    }
}
