using EasyRpc.Core.Util;
using Grpc.Core;

namespace EasyRpc.Master
{
    public partial class EasyRpcService
    {
        private readonly string _serviceHost;
        private readonly int _port;
        private Server? _masterServer;

        public bool IsConnected => _masterServer != null;

        public void Start()
        {
            SetupMasterServer();
            foreach (var plugin in _plugins)
                plugin.Load();
        }

        public void Stop()
        {
            Task.WaitAll(_masterServer!.ShutdownAsync());
            _masterServer = null;
            foreach (var plugin in _plugins)
                plugin.Unload();
        }

        private void SetupMasterServer()
        {
            _masterServer = new Server
            {
                Services = { MasterService.BindService(new EasyRpcMasterService(Register, Unregister, Notify)) },
                Ports = {
                    new ServerPort(
                        _serviceHost,
                        _port,
                        GrpcChannelSecurityHelper.GetSecureServerCredentials(_serverCertificateProvider))
                }
            };

            _masterServer.Start();
            System.Diagnostics.Debug.WriteLine($"Master server listening on port {_port}");
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
