using EasyRpc.Core.Base;
using EasyRpc.Core.Plugin;
using EasyRpc.Master;
using EasyRpc.Master.PeerManagement;
using EasyRpc.Plugin.SignalR;

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
                    Console.WriteLine("[SERVER APP] Peer added: " + peerInfo);
                };

            //Start all the services and load all the plugins
            service.Start();

            Console.WriteLine("Service started. Press any key to send a message...");
            Console.ReadKey();

            var response = service.MakeRequest(
                        new EasyRpc.Types.Message()
                        {
                            From = service.Id,
                            To = service.PeerList.First().Id,
                            Id = Guid.NewGuid().ToString(),
                            Type = EasyRpc.Types.MessageType.Request,
                            Data = "Welcome to the world of EasyRPC!!!"
                        });

            Console.WriteLine("[SERVER APP] Response: " + response.Result);

            Console.WriteLine("Press any key to stop the service...");
            Console.ReadKey();

            //Stop all services and unload the plugins
            service.Stop();
        }
    }
}
