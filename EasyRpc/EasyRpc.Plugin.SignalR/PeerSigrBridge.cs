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
        private readonly IServiceProvider _serviceProvider;
        private IMasterService? _masterHandle;

        private IMasterService MasterHandle => _masterHandle ??= _serviceProvider.GetService<IMasterService>() ?? throw new TypeInitializationException("MasterService not initialized", null);

        public PeerSigrBridge(IServiceProvider serviceProvider, SignalRPeerHub hub, ISigrPeerClientStore clientStore)
        {
            _serviceProvider = serviceProvider;
            _hub = hub;
            _hub.SetupHandlers(RegisterHandler, UnregisterHandler, NotifyHandler);
            _clientStore = clientStore;
        }

        private async Task RegisterHandler(string connectionId, RegistrationRequestSigr registrationReq)
        {
            //We need to add the client first
            _clientStore.AddClient(connectionId, registrationReq);
            var sample = _clientStore.GetRegistration(connectionId);

            RegistrationRequest registerationRequest = registrationReq;
            //TODO: Change this to something unique and different then connection id
            registerationRequest.Properties.Add(CommonConstants.SigrReferenceTag, connectionId);
            var result = await MasterHandle.Register(registerationRequest);

            if (result.Status != "Success")
            {
                //If the addition failed then remove it and fault.
                _clientStore.RemoveClient(connectionId);
                throw new Exception("Registration failed");
            }

            registrationReq.RegistrationId = result.RegistrationId;

            await _hub.Clients
                .Client(connectionId)
                .SendAsync(nameof(IEasyRpcSignalRHub.SendRegisterResponse),
                new RegistrationResponseSigr() { RegistrationId = result.RegistrationId, Status = result.Status, Message = result.Message });
        }

        private Task UnregisterHandler(string connectionId, RegistrationRequestSigr request)
        {
            //TODO: Check if we need to remove the registration info on unregister for now ignoring it
            //TODO: Check if we need to respond back to the client
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
