using CommServices.CommShared;

namespace CommPeerServices.Base.Client
{
    public interface IPeerClient
    {
        Task<Message> MakeRequest(Message message);

        Task<Empty> Notify(Message message);
    }
}
