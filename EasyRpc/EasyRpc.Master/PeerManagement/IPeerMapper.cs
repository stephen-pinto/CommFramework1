using EasyRpc.Master.PeerBase;

namespace EasyRpc.Master.PeerManagement
{
    public interface IPeerMapper
    {
        bool HasAnyCriteria { get; }
        void AddCriteria(IPeerMappingCriteria criteria);
        PeerInfo MapPeer(PeerInfo sourcePeer);
        void RemoveCriteria(IPeerMappingCriteria criteria);
    }
}
