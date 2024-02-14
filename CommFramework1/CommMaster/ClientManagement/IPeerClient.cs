using CommServices.CommShared;

namespace CommMaster.ClientManagement
{
    public interface IPeerClient
    {
        Task<Message> MakeRequest(Message message);

        Task<Message> Notify(Message message);
    }
}
