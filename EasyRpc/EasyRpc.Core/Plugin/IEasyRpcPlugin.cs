using EasyRpc.Core.Client;

namespace EasyRpc.Core.Plugin
{
    public interface IEasyRpcPlugin
    {
        public void Init(IEasyRpcPluginConfiguration config);

        public void Load();

        public void Unload();

        public IPeerClientFactory GetClientFactory();
    }
}
