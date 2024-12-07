using EasyRpc.Core.Client;

namespace EasyRpc.Core.Plugin
{
    public interface IEasyRpcPlugin
    {
        string TypeIdentifier { get; }

        void Init(IEasyRpcPluginConfiguration config);

        void Load();

        void Unload();

        IPeerClientFactory GetClientFactory();

        void SetupServiceProvider(IEasyRpcService serviceProvider);
    }
}
