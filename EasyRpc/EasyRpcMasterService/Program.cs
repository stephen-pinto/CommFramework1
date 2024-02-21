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
            BackendClient tempClient = new BackendClient();
            IEasyRpcPlugin sigrPlugin = new SignalRPlugin();
            sigrPlugin.Init(new SignalRPluginConfiguration());
            DefaultPeerClientResolver resolver = new DefaultPeerClientResolver
            {
                { "BackendClient", new BackendClientFactory(tempClient) }                
            };

            IEasyRpcServices service = new EasyRpcService("localhost", 50051, resolver);
            IMasterClient masterClient = service;
            IPeerClient peerClient = service;

            sigrPlugin.Load();
            service.UsePlugin(sigrPlugin);
            service.Start();

            var info = masterClient.Register(new RegistrationRequest
            {
                Name = "BackendClient",
                Type = "BackendClient",
                Address = string.Empty,
                Properties = { { "key", "value" } },
                RegistrationId = string.Empty
            }).GetAwaiter().GetResult();

            peerClient.MakeRequest(new Message
            {
                From = "BackendClient",
                To = info.RegistrationId,
                Data = "Hello from MasterService!"
            });

            Console.WriteLine("Press any key to stop the service...");
            Console.ReadKey();

            sigrPlugin.Unload();
            service.Stop();

            //SignalRPeerService.SignalRService service = new SignalRPeerService.SignalRService();
            //service.Start();
        }
    }
}