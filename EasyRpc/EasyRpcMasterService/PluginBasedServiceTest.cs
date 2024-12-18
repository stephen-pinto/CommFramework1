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
            sigrPlugin.Init(new SignalRPluginConfiguration());

            service.UsePlugin(sigrPlugin);

            service.PeerAdded += (sender, peerInfo) =>
                {
                    Task.Run(() =>
                    {
                        Thread.Sleep(3000);
                        service.MakeRequest(
                        new EasyRpc.Types.Message()
                        {
                            From = service.Id,
                            To = peerInfo.Id,
                            Id = Guid.NewGuid().ToString(),
                            Type = EasyRpc.Types.MessageType.Request,
                            Data = "Welcome to the world of EasyRPC!!!"
                        });
                    });
                };

            //Start all the services and load all the plugins
            service.Start();

            Console.WriteLine("Press any key to stop the service...");
            Console.ReadKey();

            //Stop all services and unload the plugins
            service.Stop();
        }
    }
}
