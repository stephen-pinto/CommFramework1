using CommServices.CommMaster;

namespace CommPeerServices.Base.Client
{
    public interface IPeerClientFactory
    {
        IPeerClient GetHandle(RegisterationRequest registerationRequest);
    }
}
