using EasyRpc.Core.Client;

namespace EasyRpc.Master.PeerManagement
{
    public record PeerRegistryEntry(PeerInfo Peer, IPeerService Handle);
}
