using CommMaster.PeerClient;
using CommPeerServices.Base.Client;

namespace CommMaster.PeerManagement
{
    public record PeerRegistryEntry(Peer Peer, IPeerClient Handle);
}
