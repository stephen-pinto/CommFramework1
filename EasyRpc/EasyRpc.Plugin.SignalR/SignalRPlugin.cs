using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Plugin.SignalR.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRpc.Plugin.SignalR
{
    public class SignalRPlugin : IEasyRpcPlugin
    {
        private WebApplication? _app;

        public string TypeIdentifier => "SignalRClient";

        public void Init(IEasyRpcPluginConfiguration config)
        {
            var sconfig = (SignalRPluginConfiguration)config;
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddCors(options => options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed((_) => true).AllowCredentials();
                }));
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<ResponseAwaiter>();
            builder.Services.AddSingleton(sconfig.MasterClient!);
            builder.Services.AddSingleton(sconfig.MainPeerClient!);
            builder.Services.AddSingleton<ISigrPeerClientStore, DefaultSigrPeerClientStore>();
            builder.Services.AddSingleton<IPeerClientFactory, SigrPeerClientFactory>();
            _app = builder.Build();
            _app.UseHttpsRedirection();
            _app.UseStaticFiles();
            _app.UseRouting();
            _app.MapHub<SignalRPeerHub>("/peer");
            _app.UseCors("AllowAll");
            _app.Urls.Add("https://localhost:5001");
        }

        public void Load()
        {
            _app!.Run();
        }

        public void Unload()
        {
            _app!.StopAsync().Wait();
        }

        public IPeerClientFactory GetClientFactory()
        {
            return _app!.Services.GetService<IPeerClientFactory>()!;
        }
    }
}
