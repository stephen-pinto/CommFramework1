using CommPeerServices.Base.Client;
using Microsoft.AspNetCore.SignalR;

namespace SignalRPeerService.Old
{
    internal class SigrClientRegister
    {
        private Dictionary<string, IPeerClient> _clientStore;

        public SigrClientRegister()
        {
            _clientStore = new Dictionary<string, IPeerClient>();
        }

        public IPeerClient GetProxyClientForConnection(Hub hub, string connectionId)
        {
            //if(_clientStore.TryGetValue(connectionId, out IPeerClient client))
            //{
            //    return client;
            //}

            //var newClient = new PeerSignalrClient(hub, connectionId);
            //_clientStore.Add(connectionId, newClient);
            //return newClient;
            throw new NotImplementedException();
        }

        public IPeerClient RemoveClient(string connectionId)
        {
            _clientStore.Remove(connectionId, out var client);
            return client;
        }
    }
}
