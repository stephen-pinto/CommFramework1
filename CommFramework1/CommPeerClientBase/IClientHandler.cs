using CommServices.CommPeer;

namespace CommPeerClientBase
{
    public interface IClientHandler
    {
        CommPeerService.CommPeerServiceClient Client { get; }

        void Start(string address);
    }
}
