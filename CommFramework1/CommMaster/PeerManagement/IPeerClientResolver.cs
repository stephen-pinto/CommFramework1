using CommPeerServices.Base.Client;
using CommServices.CommMaster;

namespace CommMaster.PeerManagement
{
    public interface IPeerClientResolver
    {
        IPeerClient GetHandle(RegisterationRequest request);
    }
}
