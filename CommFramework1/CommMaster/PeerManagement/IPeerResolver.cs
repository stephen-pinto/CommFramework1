using CommMaster.PeerClient;
using CommServices.CommMaster;

namespace CommMaster.PeerManagement
{
    internal interface IPeerHandlerResolver
    {
        IPeerClient GetHandle(RegisterationRequest request);
    }
}
