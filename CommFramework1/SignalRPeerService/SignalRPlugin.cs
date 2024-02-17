using CommPeerServices.Base.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRPeerService
{
    public class SignalRPlugin : ICommPlugin
    {
        private WebApplication? _app;

        public void Init(IPluginConfiguration config)
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
            _app = builder.Build();
            _app.UseHttpsRedirection();
            _app.UseStaticFiles();
            _app.UseRouting();
            _app.MapHub<PeerHub>("/peer");
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
    }
}
