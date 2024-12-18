using EasyRpc.Core.Base;
using EasyRpc.Core.Client;

namespace EasyRpc.Core.Plugin
{
    public interface IEasyRpcPlugin
    {
        public string TypeIdentifier { get; }

        public void Init(IEasyRpcPluginConfiguration config);

        public void Load(IMasterService masterService);

        public void Unload();

        public IPeerClientFactory GetClientFactory();
    }
}
