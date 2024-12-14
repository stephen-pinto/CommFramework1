using EasyRpc.Core.Plugin;

namespace EasyRpc.Core.Client
{
    public interface IEasyRpcServices : IMasterService, IPeerService
    {
        void Start();

        void Stop();

        IEasyRpcServices UsePlugin(IEasyRpcPlugin plugin);
    }
}
