namespace CommMaster.ClientManagement
{
    public record PeerRegistryEntry(string Id, Peer Peer, IPeerClient Handle);
}
