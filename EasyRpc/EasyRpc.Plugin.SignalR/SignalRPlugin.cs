﻿using EasyRpc.Core.Client;
using EasyRpc.Core.Plugin;
using EasyRpc.Plugin.SignalR.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;

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

            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates.OfType<X509Certificate2>()
                .First(c => c.FriendlyName ==
                "SigR Server Authorization");

            _builder.Host.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                   .UseKestrel(options =>
                   {
                       options.Listen(System.Net.IPAddress.Loopback, 55155, listenOptions =>
                       {
                           var connectionOptions = new HttpsConnectionAdapterOptions();
                           connectionOptions.ServerCertificate = certificate;
                           listenOptions.UseHttps(connectionOptions);
                       });
                   });
            });

            _builder.Services.AddSignalR();
            _builder.Services.AddSingleton<ResponseAwaiter>();
            _builder.Services.AddSingleton(sconfig.MasterClient!);
            _builder.Services.AddSingleton<ISigrPeerClientStore, DefaultSigrPeerClientStore>();
            _builder.Services.AddSingleton<IPeerClientFactory, SigrPeerClientFactory>();
            _app = _builder.Build();
            _app.UseHttpsRedirection();
            _app.UseStaticFiles();
            _app.UseRouting();
            _app.MapHub<SignalRPeerHub>("/peer");
            _app.UseCors("AllowAll");
            //_app.Urls.Add("https://localhost:5001");
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
