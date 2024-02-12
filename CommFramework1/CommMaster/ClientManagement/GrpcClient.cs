//using CommPeerClientBase;

//namespace CommMaster.ClientManagement
//{
//    public class GrpcClient : IClientHandle
//    {
//        private IClientHandler _handler;

//        public GrpcClient(string address, IClientHandler handler)
//        {
//            _handler = handler;
//            _handler.Start(address);
//        }

//        public async Task<PeerMessage> MakeRequest(PeerMessage request)
//        {
//            return await _handler.Client.MakeRequestAsync(request);            
//        }

//        public async Task<PeerMessage> Notify(PeerMessage request)
//        {
//            return await _handler.Client.NotifyAsync(request);
//        }
//    }
//}
