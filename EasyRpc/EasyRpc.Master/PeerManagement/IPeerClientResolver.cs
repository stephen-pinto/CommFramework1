using EasyRpc.Core.Base;
using EasyRpc.Core.Client;

namespace EasyRpc.Master.PeerManagement
{
    public interface IPeerClientResolver
    {
        void AddFactory(string identifier, IPeerClientFactory peerClientFactory);

        IPeerService GetHandle(RegistrationRequest request);
    }
}
