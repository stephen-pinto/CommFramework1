using CommMaster.Exceptions;
using CommMaster.PeerClient;

namespace CommMaster.PeerManagement
{
    internal class PeerMapper : IPeerMapper
    {
        private List<IPeerMappingCriteria> _criterias;

        public PeerMapper()
        {
            _criterias = new List<IPeerMappingCriteria>();
        }

        public bool HasAnyCriteria => _criterias.Any();

        public void AddCriteria(IPeerMappingCriteria criteria)
        {
            _criterias.Add(criteria);
        }

        public Peer MapPeer(Peer sourcePeer)
        {
            foreach (var criteria in _criterias)
            {
                if (criteria.TryGetMatchingPeer(sourcePeer, out Peer matchedPeer))
                {
                    return matchedPeer;
                }
            }

            throw new PeerMappingException("No matching peer found");
        }

        public void RemoveCriteria(IPeerMappingCriteria criteria)
        {
            _criterias.Remove(criteria);
        }
    }
}
