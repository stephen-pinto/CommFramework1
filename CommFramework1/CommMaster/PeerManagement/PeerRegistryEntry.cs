using CommMaster.PeerClient;

namespace CommMaster.PeerManagement
{
    public record PeerRegistryEntry(string Id, Peer Peer, IPeerClient Handle);
}
