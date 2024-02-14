using CommServices.CommShared;

namespace CommMaster.PeerClient
{
    public interface IPeerClient
    {
        Task<Message> MakeRequest(Message message);

        Task<Message> Notify(Message message);
    }
}
