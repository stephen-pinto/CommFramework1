using CommPeerServices.Base.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRPeerService
{
    public class SignalRService
    {
        private WebApplication? _app;

        public void Start(IMasterClient masterClient, IPeerClient mainPeerClient)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddCors(options => options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed((_) => true).AllowCredentials();
                }));
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<ResponseAwaiter>();
            builder.Services.AddSingleton(masterClient);
            builder.Services.AddSingleton(mainPeerClient);
            _app = builder.Build();
            _app.UseHttpsRedirection();
            _app.UseStaticFiles();
            _app.UseRouting();
            _app.MapHub<PeerHub>("/peer");
            _app.UseCors("AllowAll");
            _app.Urls.Add("https://localhost:5001");
            _app.Run();
        }

        public void Stop()
        {
            _app!.StopAsync().Wait();
        }
    }
}
