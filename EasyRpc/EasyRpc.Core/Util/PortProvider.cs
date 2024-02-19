using System.Net;
using System.Net.Sockets;

namespace EasyRpc.Core.Util
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
