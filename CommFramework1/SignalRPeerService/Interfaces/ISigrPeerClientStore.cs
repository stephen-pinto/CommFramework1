using CommPeerServices.Base.Client;
using SignalRPeerService.Types;

namespace SignalRPeerService.Interfaces
{
    public interface ISigrPeerClientStore
    {
        IPeerClient GetClient(string connectionId);
        IPeerClient AddNewRegisteredClient(string connectionId, RegisterationRequestSigr registration); 
        void RemoveClient(string connectionId);
    }
}