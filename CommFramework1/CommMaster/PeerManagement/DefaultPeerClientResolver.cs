using CommPeerServices.Base.Client;
using CommServices.CommMaster;

namespace CommMaster.PeerManagement
{
    /// <summary>
    /// This resolver refers to the client's type to resolve to appropriate factory
    /// </summary>
    public class DefaultPeerClientResolver : Dictionary<string, IPeerClientFactory>, IPeerClientResolver
    {
        public IPeerClient GetHandle(RegisterationRequest request)
        {
            if (ContainsKey(request.Type))
                return this[request.Type].GetHandle(request);

            throw new NotSupportedException($"Peer type {request.Type} is not supported");
        }
    }
}
