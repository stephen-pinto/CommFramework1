using EasyRpc.Core.Plugin;
using EasyRpc.Master.PeerManagement;
using EasyRpc.Master;
using EasyRpc.Plugin.SignalR;
using EasyRpc.Core.Base;

namespace EasyRpcMasterService
{
    internal class PluginBasedServiceTest
    {
        public static void Run()
        {
            IEasyRpcServices service = new EasyRpcService(new Uri("https://localhost:50001"), new DefaultPeerClientResolver());

            //Setup plugins to use the main service directly
            IEasyRpcPlugin sigrPlugin = new SignalRPlugin();
            sigrPlugin.Init(new SignalRPluginConfiguration() { MasterClient = service, MainPeerClient = service });

            //var backendPlugin = new BackendClientPlugin();
            //backendPlugin.Init(new BackendPluginConfiguration() { MasterClient = service, MainPeerClient = service });

            service.UsePlugin(sigrPlugin);
            //service.UsePlugin(backendPlugin);

            //Start all the services and load all the plugins
            service.Start();

            //Set sending signals
            //backendPlugin.Test();

            Console.WriteLine("Press any key to stop the service...");
            Console.ReadKey();

            //Stop all services and unload the plugins
            service.Stop();

            //SignalRPeerService.SignalRService service = new SignalRPeerService.SignalRService();
            //service.Start();
        }
    }
}
