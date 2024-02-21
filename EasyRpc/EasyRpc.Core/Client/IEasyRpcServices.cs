using EasyRpc.Core.Plugin;

namespace EasyRpc.Core.Client
{
    public interface IEasyRpcServices : IMasterClient, IPeerClient
    {
        void Start();

        void Stop();

        void UsePlugin(IEasyRpcPlugin plugin);
    }
}
