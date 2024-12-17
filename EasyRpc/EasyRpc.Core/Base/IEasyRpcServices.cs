using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Types;

namespace EasyRpc.Core.Base
{
    public interface IEasyRpcServices : IMasterService, IPeerService
    {
        event EventHandler<Message> Notification;

        event EventHandler<PeerInfo> PeerAdded;

        event EventHandler PeerRemoved;

        IReadOnlyCollection<PeerInfo> PeerList { get; }

        void Start();

        void Stop();

        IEasyRpcServices UsePlugin(IEasyRpcPlugin plugin);
    }
}
