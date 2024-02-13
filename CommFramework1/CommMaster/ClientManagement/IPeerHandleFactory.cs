using CommServices.CommMaster;

namespace CommMaster.ClientManagement
{
    internal interface IPeerHandleFactory
    {
        IPeerHandle GetHandle(RegisterationRequest registerationRequest);
    }
}
