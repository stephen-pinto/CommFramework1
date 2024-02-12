using CommMaster.PeerClient;
using CommServices.CommShared;

namespace CommMaster.ClientManagement
{
    public class GrpcPeer : IPeerHandle
    {
        private IClientHandler _handler;

        public GrpcPeer(string address, IClientHandler handler)
        {
            _handler = handler;
            _handler.Start(address);
        }

        public async Task<Message> MakeRequest(Message request)
        {
            return await _handler.Client.MakeRequestAsync(request);
        }

        public async Task<Message> Notify(Message request)
        {
            return await _handler.Client.NotifyAsync(request);
        }
    }
}
