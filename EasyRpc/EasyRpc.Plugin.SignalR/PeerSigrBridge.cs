using EasyRpc.Core.Base;
using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRpc.Plugin.SignalR
{
    public class PeerSigrBridge
    {
        private readonly SignalRPeerHub _peerHub;
        private readonly ISigrPeerClientStore _clientStore;
        public event NotifyDelegate? Notify;
        private readonly IEasyRpcSignalRHub _refInterface;
        private readonly IServiceProvider _serviceProvider;
        private IMasterService _masterHandle;

        private IMasterService MasterHandle => _masterHandle ??= _serviceProvider.GetService<IMasterService>() ?? throw new TypeInitializationException("MasterService not initialized", null);

        public PeerSigrBridge(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _peerHub = serviceProvider.GetService<SignalRPeerHub>() ?? throw new TypeInitializationException("PeerHub not initialized", null);
            _peerHub!.SetupHandlers(RegisterHandler, UnregisterHandler, MakeRequestHandler, NotifyHandler);
            _refInterface = _peerHub;
            _clientStore = _serviceProvider.GetService<ISigrPeerClientStore>() ?? throw new TypeInitializationException("ISigrPeerClientStore not initialized", null);
        }

        private Task RegisterHandler(string connectionId, RegistrationRequestSigr request)
        {
            _clientStore.AddNewRegisteredClient(connectionId, request);
            return Task.CompletedTask;
        }

        private Task UnregisterHandler(string connectionId, RegistrationRequestSigr request)
        {
            _clientStore.RemoveClient(connectionId);
            return Task.CompletedTask;
        }

        private async Task NotifyHandler(string connectionId, MessageSigr message)
        {
            if (Notify != null)
                await Notify(message);
        }

        private async Task MakeRequestHandler(string connectionId, MessageSigr message)
        {
            //TODO: We dont need to handle it at master end
            await Task.FromException<NotImplementedException>(new NotImplementedException("MakeRequestHandler not implemented"));
        }
    }
}
