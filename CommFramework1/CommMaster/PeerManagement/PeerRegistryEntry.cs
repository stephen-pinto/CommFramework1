using CommMaster.PeerClient;
using CommPeerServices.Base.Client;

namespace CommMaster.PeerManagement
{
    public record PeerRegistryEntry(string Id, Peer Peer, IPeerClient Handle);
}
