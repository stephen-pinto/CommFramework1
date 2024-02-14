using CommMaster.PeerClient;

namespace CommMaster.PeerManagement
{
    public class DefaultPeerMappingCriteria : IPeerMappingCriteria
    {
        private readonly IPeerRegistry _registry;

        public DefaultPeerMappingCriteria(IPeerRegistry registry)
        {
            _registry = registry;
        }

        public bool TryGetMatchingPeer(Peer sourcePeer, out Peer matchedPeer)
        {
            ReadOnlySpan<PeerRegistryEntry> peers = new ReadOnlySpan<PeerRegistryEntry>(_registry.Values.ToArray());
            foreach (var item in peers)
            {
                if (item.Peer.Type == sourcePeer.Type)
                {
                    matchedPeer = item.Peer;
                    return true;
                }
            }

            matchedPeer = null!;
            return false;
        }
    }
}
