using EasyRpc.Core.Client;

namespace EasyRpc.Master.PeerManagement
{
    /// <summary>
    /// This resolver refers to the client's type to resolve to appropriate factory
    /// </summary>
    public class DefaultPeerClientResolver : Dictionary<string, IPeerClientFactory>, IPeerClientResolver
    {
        public void AddFactory(string identifier, IPeerClientFactory peerClient)
        {
            this.Add(identifier, peerClient);
        }

        public IPeerClient GetHandle(RegistrationRequest request)
        {
            if (ContainsKey(request.Type))
                return this[request.Type].GetHandle(request);

            throw new NotSupportedException($"Peer type {request.Type} is not supported");
        }
    }
}