using EasyRpc.Types;

namespace EasyRpc.Core.Client
{
    public delegate Task<Message> MakeRequestDelegate(Message message);
    public delegate Task<Empty> NotifyDelegate(Message message);

    public interface IPeerClient
    {
        Task<Message> MakeRequest(Message message);

        Task<Empty> Notify(Message message);
    }
}
