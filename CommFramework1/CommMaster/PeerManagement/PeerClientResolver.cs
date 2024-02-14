using CommMaster.PeerClient;
using CommServices.CommMaster;

namespace CommMaster.PeerManagement
{
    internal class PeerClientResolver : Dictionary<string, IPeerClientFactory>
    {
        public IPeerClient GetHandle(RegisterationRequest request)
        {
            if(ContainsKey(request.Type))
                return this[request.Type].GetHandle(request);

            throw new NotSupportedException($"Peer type {request.Type} is not supported");
        }
    }
}
