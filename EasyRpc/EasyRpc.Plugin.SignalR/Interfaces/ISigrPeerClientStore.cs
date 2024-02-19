using EasyRpc.Core.Client;
using EasyRpc.Plugin.SignalR.Types;

namespace EasyRpc.Plugin.SignalR.Interfaces
{
    public interface ISigrPeerClientStore
    {
        IPeerClient GetClient(string connectionId);
        IPeerClient AddNewRegisteredClient(string connectionId, RegistrationRequestSigr registration);
        void RemoveClient(string connectionId);
    }
}
