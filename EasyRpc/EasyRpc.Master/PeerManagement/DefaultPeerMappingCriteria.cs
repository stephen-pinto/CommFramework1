using EasyRpc.Core.Client;

namespace EasyRpc.Master.PeerManagement
{
    public class DefaultPeerMappingCriteria : IPeerMappingCriteria
    {
        private readonly IPeerRegistry _registry;

        public DefaultPeerMappingCriteria(IPeerRegistry registry)
        {
            _registry = registry;
        }

        public bool TryGetMatchingPeer(PeerInfo sourcePeer, out PeerInfo? matchedPeer)
        {
            var peers = new ReadOnlySpan<PeerRegistryEntry>(_registry.Values.ToArray());
            foreach (var item in peers)
            {
                if (item.Peer.Type == sourcePeer.Type)
                {
                    matchedPeer = item.Peer;
                    return true;
                }
            }

            matchedPeer = null;
            return false;
        }

        public bool TryGetMatchingPeer(Func<PeerInfo, bool> matchCondition, out PeerInfo? matchedPeer)
        {
            var peers = new ReadOnlySpan<PeerRegistryEntry>(_registry.Values.ToArray());
            foreach (var item in peers)
            {
                if(matchCondition(item.Peer))
                {
                    matchedPeer = item.Peer;
                    return true;
                }
            }

            matchedPeer = null;
            return false;
        }
    }
}
