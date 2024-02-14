using CommPeerGrpcNetService;
using CommServices.CommMaster;

namespace CommMaster.ClientManagement
{
    public interface IPeerClientFactory
    {
        IPeerClient GetHandle(RegisterationRequest registerationRequest);
    }
}
