using EasyRpc.Core.Plugin;

namespace EasyRpc.Core.Client
{
    public interface IEasyRpcService : IMasterClient, IPeerClient
    {
        void Start();

        void Stop();

        IEasyRpcService UsePlugin(IEasyRpcPlugin plugin);
    }
}
