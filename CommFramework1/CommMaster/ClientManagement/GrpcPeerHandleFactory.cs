using CommPeerGrpcNetService;
using CommServices.CommMaster;

namespace CommMaster.ClientManagement
{
    internal class GrpcPeerHandleFactory : IPeerClientFactory
    {
        public IPeerClient GetHandle(RegisterationRequest registerationRequest)
        {
            return new GrpcPeerClient(registerationRequest.Address);
        }
    }
}
