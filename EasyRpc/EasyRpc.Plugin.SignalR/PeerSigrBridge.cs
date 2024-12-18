using EasyRpc.Core.Base;
using EasyRpc.Master;
using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRpc.Plugin.SignalR
{
    public class PeerSigrBridge
    {
        private readonly SignalRPeerHub _hub;
        private readonly ISigrPeerClientStore _clientStore;
        public event NotifyDelegate? Notify;
        private readonly IEasyRpcSignalRHub _refInterface;
        private readonly IServiceProvider _serviceProvider;
        private IMasterService? _masterHandle;

        private IMasterService MasterHandle => _masterHandle ??= _serviceProvider.GetService<IMasterService>() ?? throw new TypeInitializationException("MasterService not initialized", null);

        public PeerSigrBridge(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _refInterface = _hub = serviceProvider.GetService<SignalRPeerHub>() ?? throw new TypeInitializationException("PeerHub not initialized", null);
            _hub.SetupHandlers(RegisterHandler, UnregisterHandler, NotifyHandler);
            _clientStore = _serviceProvider.GetService<ISigrPeerClientStore>() ?? throw new TypeInitializationException("ISigrPeerClientStore not initialized", null);
        }

        private Task RegisterHandler(string connectionId, RegistrationRequestSigr registrationReq)
        {
            RegistrationRequest registerationRequest = registrationReq;
            //TODO: Change this to something unique and different then connection id
            registerationRequest.Properties.Add(CommonConstants.SigrReferenceTag, connectionId);
            var result = MasterHandle.Register(registerationRequest).GetAwaiter().GetResult();

            if (result.Status != "Success")
                throw new Exception("Registration failed");

            registrationReq.RegistrationId = result.RegistrationId;

            _hub.Clients
                .Client(connectionId)
                .SendAsync(nameof(SignalRPeerHub.SendRegisterResponse),
                new RegistrationResponseSigr() { RegistrationId = result.RegistrationId, Status = result.Status, Message = result.Message });

            _clientStore.AddClient(connectionId, registrationReq);
            return Task.CompletedTask;
        }

        private Task UnregisterHandler(string connectionId, RegistrationRequestSigr request)
        {
            //TODO: Check if we need to remove the registration info on unregister for now ignoring it
            var registration = _clientStore.GetRegistration(connectionId);

            var result = MasterHandle.Unregister(request).GetAwaiter().GetResult();
            if (result.Status != "Success")
                throw new Exception("Unregistration failed");

            _clientStore.RemoveClient(connectionId);
            return Task.CompletedTask;
        }

        private async Task NotifyHandler(string connectionId, MessageSigr message)
        {
            await MasterHandle.Notify(message);
        }
    }
}
