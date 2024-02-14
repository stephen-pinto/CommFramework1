using CommServices.CommMaster;

namespace CommMaster.PeerClient
{
    internal class GrpcPeerClientFactory : IPeerClientFactory
    {
        public IPeerClient GetHandle(RegisterationRequest registerationRequest)
        {
            return new GrpcPeerClient(registerationRequest.Address);
        }
    }
}
