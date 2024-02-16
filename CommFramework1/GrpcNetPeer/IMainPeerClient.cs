using CommPeerServices.Base.Client;

namespace GrpcNetPeer
{
    internal interface IMainPeerClient : IPeerClient, IMasterClient
    {
    }
}
