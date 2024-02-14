using CommServices.CommMaster;

namespace CommMaster.PeerClient
{
    public interface IPeerClientFactory
    {
        IPeerClient GetHandle(RegisterationRequest registerationRequest);
    }
}
