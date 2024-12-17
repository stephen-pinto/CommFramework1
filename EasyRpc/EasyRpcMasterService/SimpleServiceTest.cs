using EasyRpc.Core.Base;
using EasyRpc.Master;
using EasyRpc.Master.PeerManagement;
using EasyRpc.Types;

namespace EasyRpcMasterService
{
    internal class SimpleServiceTest
    {
        public static void Run()
        {
            var resolver = new DefaultPeerClientResolver();

            IEasyRpcServices service = new EasyRpcService(
                new Uri("https://localhost:5001"), 
                new DefaultPeerClientResolver());

            //Start all the services and load all the plugins
            service.Start();

            service.Notification += Service_Notification;

            Console.WriteLine("Press any key to send a message...");
            Console.ReadKey();

            var peer = service.PeerList.First();
            var msg = new Message() { To = peer.Id, Data = "Order from master", From = service.Id, Type = MessageType.Request, Id = Guid.NewGuid().ToString() };
            service.MakeRequest(msg);

            Console.WriteLine("Press any key to stop the service...");
            Console.ReadKey();


            //Stop all services and unload the plugins
            service.Stop();
        }

        private static void Service_Notification(object? sender, EasyRpc.Types.Message e)
        {
            Console.WriteLine($"[MASTER] Received notification: {e.Data}");
        }
    }
}
