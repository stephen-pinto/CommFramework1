using EasyRpc.Types;

namespace EasyRpc.Core.Client
{
    public interface IEasyRpcServicesPluginAbstraction : IEasyRpcServices
    {
        void RaiseNotification(Message message);
    }
}