using EasyRpc.Core.Client;
using EasyRpc.Master.PeerBase;

namespace EasyRpc.Master.PeerManagement
{
    public record PeerRegistryEntry(PeerInfo Peer, IPeerClient Handle);
}
