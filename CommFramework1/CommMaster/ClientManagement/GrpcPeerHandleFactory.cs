using CommMaster.PeerClient;
using CommServices.CommMaster;

namespace CommMaster.ClientManagement
{
    internal class GrpcPeerHandleFactory : IPeerHandleFactory
    {
        public IPeerHandle GetHandle(RegisterationRequest registerationRequest)
        {
            return new GrpcPeer(registerationRequest.Address, new DefaultClientHandler());
        }
    }
}
