using CommMaster;
using CommMaster.PeerManagement;
using CommPeerServices.Base.Client;
using CommPeerServices.Base.Plugins;
using CommServices.CommShared;
using SignalRPeerService;

namespace MasterService
{
    class Program
    {
        static void Main(string[] args)
        {
            BackendClient tempClient = new BackendClient();
            ICommPlugin sigrPlugin = new SignalRPlugin();
            sigrPlugin.Init(new SignalRPluginConfiguration());
            DefaultPeerClientResolver resolver = new DefaultPeerClientResolver
            {
                { "BackendClient", new BackendClientFactory(tempClient) },
                { "SignalRClient", sigrPlugin.GetClientFactory()}
            };

            CommService service = new CommService("localhost", 50051, resolver);
            IMasterClient masterClient = service;
            IPeerClient peerClient = service;

            sigrPlugin.Load();
            service.Start();

            var info = masterClient.Register(new CommServices.CommMaster.RegisterationRequest
            {
                Name = "BackendClient",
                Type = "BackendClient",
                Address = string.Empty,
                Properties = { { "key", "value" } },
                RegistrationId = string.Empty
            }).GetAwaiter().GetResult();

            peerClient.MakeRequest(new CommServices.CommShared.Message
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