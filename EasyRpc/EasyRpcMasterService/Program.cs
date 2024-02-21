using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Master;
using EasyRpc.Master.PeerManagement;
using EasyRpc.Plugin.SignalR;
using EasyRpc.Types;
using EasyRpcMasterService;

namespace MasterService
{
    class Program
    {
        static void Main(string[] args)
        {
            IEasyRpcServices service = new EasyRpcService("localhost", 50051, new DefaultPeerClientResolver());

            IEasyRpcPlugin sigrPlugin = new SignalRPlugin();
            sigrPlugin.Init(new SignalRPluginConfiguration() { MasterClient = service, MainPeerClient = service });

            var backendPlugin = new BackendClientPlugin();
            backendPlugin.Init(new BackendPluginConfiguration() { MasterClient = service, MainPeerClient = service });

            //IMasterClient masterClient = service;
            //IPeerClient peerClient = service;

            service.UsePlugin(sigrPlugin);
            service.UsePlugin(backendPlugin);
            service.Start();

            backendPlugin.Test();

            Console.WriteLine("Press any key to stop the service...");
            Console.ReadKey();

            service.Stop();

            //SignalRPeerService.SignalRService service = new SignalRPeerService.SignalRService();
            //service.Start();
        }
    }
}