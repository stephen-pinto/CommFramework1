using CommServices.CommMaster;

namespace CommMaster.ClientManagement
{
    internal class PeerHandlerResolver : Dictionary<string, IPeerHandleFactory>
    {
        public IPeerHandle GetHandle(RegisterationRequest request)
        {
            if(ContainsKey(request.Type))
                return this[request.Type].GetHandle(request);

            throw new NotSupportedException($"Peer type {request.Type} is not supported");
        }
    }
}
