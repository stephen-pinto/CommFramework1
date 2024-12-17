using EasyRpc.Types;

namespace EasyRpc.Core.Base
{
    public delegate Task<Message> MakeRequestDelegate(Message message);

    public interface IPeerService : IRpcServiceBase, IDisposable
    {
        Task<Message> MakeRequest(Message message);
    }
}
