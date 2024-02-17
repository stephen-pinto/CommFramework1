using CommPeerServices.Base.Client;
using CommServices.CommMaster;

namespace CommMaster.PeerManagement
{
    public class PeerClientResolver : Dictionary<string, IPeerClientFactory>, IPeerClientResolver
    {
        public IPeerClient GetHandle(RegisterationRequest request)
        {
            if (ContainsKey(request.Type))
                return this[request.Type].GetHandle(request);

            throw new NotSupportedException($"Peer type {request.Type} is not supported");
        }
    }
}
