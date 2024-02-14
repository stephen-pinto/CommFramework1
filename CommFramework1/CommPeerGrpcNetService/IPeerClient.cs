using CommServices.CommShared;

namespace CommPeerGrpcNetService
{
    public interface IPeerClient
    {
        Task<Message> MakeRequest(Message message);

        Task<Message> Notify(Message message);
    }
}
