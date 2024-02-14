using CommServices.CommShared;

namespace CommPeerServices.Base.Client
{
    public interface IPeerClient
    {
        Task<Message> MakeRequest(Message message);

        Task<Message> Notify(Message message);
    }
}
