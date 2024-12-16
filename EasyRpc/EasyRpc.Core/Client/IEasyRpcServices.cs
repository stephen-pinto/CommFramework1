using EasyRpc.Core.Plugin;

namespace EasyRpc.Core.Client
{
    public interface IEasyRpcServices : IMasterService, IPeerService
    {
        IReadOnlyCollection<PeerInfo> PeerList { get; }

        void Start();

        void Stop();

        IEasyRpcServices UsePlugin(IEasyRpcPlugin plugin);
    }
}
