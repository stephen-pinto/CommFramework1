using CommClientBase;

namespace CommMaster.ClientManagement
{
    public class GrpcClient : IClientHandle
    {
        private IClientHandler _handler;

        public GrpcClient(string address, IClientHandler handler)
        {
            _handler = handler;
            _handler.Start(address);
        }

        public async Task<CommClient.Message> MakeRequest(CommClient.Message request)
        {
            return await _handler.Client.MakeRequestAsync(request);            
        }

        public async Task<CommClient.Message> Notify(CommClient.Message request)
        {
            return await _handler.Client.NotifyAsync(request);
        }
    }
}
