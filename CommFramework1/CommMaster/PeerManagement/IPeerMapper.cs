using CommMaster.PeerClient;

namespace CommMaster.PeerManagement
{
    public interface IPeerMapper
    {
        bool HasAnyCriteria { get; }
        void AddCriteria(IPeerMappingCriteria criteria);
        Peer MapPeer(Peer sourcePeer);
        void RemoveCriteria(IPeerMappingCriteria criteria);
    }
}
