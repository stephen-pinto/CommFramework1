using Grpc.Core;

namespace CommMaster.ClientManagement
{
    public interface IClientHandle
    {
        Task<CommClient.Message> MakeRequest(CommClient.Message request);

        Task<CommClient.Message> Notify(CommClient.Message request);
    }
}
