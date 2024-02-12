using CommServices.CommShared;

namespace CommMaster.ClientManagement
{
    public interface IPeerHandle
    {
        Task<Message> MakeRequest(Message request);

        Task<Message> Notify(Message request);
    }
}
