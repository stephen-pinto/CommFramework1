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
        private WebApplicationBuilder? _builder;

        public string TypeIdentifier => "SignalRClient";

        public void Init(IEasyRpcPluginConfiguration config)
        {
            var sconfig = (SignalRPluginConfiguration)config;
            _builder = WebApplication.CreateBuilder();
            _builder.Services.AddCors(options => options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed((_) => true).AllowCredentials();
                }));
            _builder.Services.AddSignalR();
            _builder.Services.AddSingleton<ResponseAwaiter>();
            _builder.Services.AddSingleton(sconfig.MasterClient!);
            _builder.Services.AddSingleton(sconfig.MainPeerClient!);
            _builder.Services.AddSingleton<ISigrPeerClientStore, DefaultSigrPeerClientStore>();
            _builder.Services.AddSingleton<IPeerClientFactory, SigrPeerClientFactory>();
            _app = _builder.Build();
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
