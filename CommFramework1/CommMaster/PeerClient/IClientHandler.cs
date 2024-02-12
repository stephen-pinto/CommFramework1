namespace CommMaster.PeerClient
{
    public interface IClientHandler
    {
        CommServices.CommPeer.CommPeerService.CommPeerServiceClient Client { get; }

        void Start(string address);
    }
}
