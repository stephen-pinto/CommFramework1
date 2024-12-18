using EasyRpc.Core.Base;
using EasyRpc.Core.Client;

namespace EasyRpc.Core.Plugin
{
    public interface IEasyRpcPlugin
    {
        void Init(IEasyRpcPluginConfiguration config);

        void Load(IMasterService masterService);

        void Unload();

        IReadOnlyDictionary<string, IPeerClientFactory> GetClientFactories();
    }
}
