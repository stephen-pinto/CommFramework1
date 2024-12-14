using EasyRpc.Types;

namespace EasyRpc.Core.Client
{
    public delegate Task<Message> MakeRequestDelegate(Message message);

    public interface IPeerService
    {
        Task<Message> MakeRequest(Message message);
    }
}
