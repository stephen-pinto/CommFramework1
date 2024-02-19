using EasyRpc.Core.Client;

namespace EasyRpc.Master.PeerManagement
{
    public interface IPeerClientResolver
    {
        IPeerClient GetHandle(RegistrationRequest request);
    }
}
