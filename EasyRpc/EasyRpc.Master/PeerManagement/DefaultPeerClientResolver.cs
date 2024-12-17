using EasyRpc.Core.Base;
using EasyRpc.Core.Client;
using EasyRpc.Master.PeerBase;

namespace EasyRpc.Master.PeerManagement
{
    /// <summary>
    /// This resolver refers to the client's type to resolve to appropriate factory
    /// </summary>
    public class DefaultPeerClientResolver : Dictionary<string, IPeerClientFactory>, IPeerClientResolver
    {
        public DefaultPeerClientResolver()
        {
            this.Add("Grpc", new GrpcPeerClientFactory());
        }

        public void AddFactory(string identifier, IPeerClientFactory peerClient)
        {
            Add(identifier, peerClient);
        }

        public IPeerService GetHandle(RegistrationRequest request)
        {
            if (ContainsKey(request.Type))
                return this[request.Type].GetHandle(request);
            else
                return this["Grpc"].GetHandle(request);
        }
    }
}