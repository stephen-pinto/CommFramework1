using CommMaster.PeerClient;

namespace CommMaster.PeerManagement
{
    public interface IPeerMappingCriteria
    {
        bool TryGetMatchingPeer(Peer sourcePeer, out Peer matchedPeer);
    }
}