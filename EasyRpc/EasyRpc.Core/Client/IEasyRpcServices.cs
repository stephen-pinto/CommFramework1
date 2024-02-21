using EasyRpc.Core.Plugin;

namespace EasyRpc.Core.Client
{
    public interface IEasyRpcServices : IMasterClient, IPeerClient
    {
        void Start();

        void Stop();

        IEasyRpcServices UsePlugin(IEasyRpcPlugin plugin);
    }
}
