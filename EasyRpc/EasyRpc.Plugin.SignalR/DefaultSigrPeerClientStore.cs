using EasyRpc.Core.Base;
using EasyRpc.Master;
using EasyRpc.Plugin.SignalR.Interfaces;
using EasyRpc.Plugin.SignalR.Types;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRpc.Plugin.SignalR
{
    public class DefaultSigrPeerClientStore : ISigrPeerClientStore
    {
        private readonly SignalRPeerHub _hub;
        private readonly ResponseAwaiter _responseAwaiter;
        private readonly Dictionary<string, IPeerService> _clients;
        private readonly IServiceProvider _serviceProvider;
        private IMasterService? _masterHandle;

        private IMasterService MasterHandle => _masterHandle ??= _serviceProvider.GetService<IMasterService>() ?? throw new TypeInitializationException("MasterService not initialized", null);

        public DefaultSigrPeerClientStore(IServiceProvider serviceProvider)
        {
            _hub = serviceProvider.GetService<SignalRPeerHub>() ?? throw new TypeInitializationException("PeerHub not initialized", null);
            _serviceProvider = serviceProvider;
            _responseAwaiter = serviceProvider.GetService<ResponseAwaiter>() ?? throw new TypeInitializationException("ResponseAwaiter not initialized", null);
            _clients = new Dictionary<string, IPeerService>();
        }

        public IPeerService GetClient(string connectionId)
        {
            return _clients[connectionId];
        }

        public IPeerService AddNewRegisteredClient(string connectionId, RegistrationRequestSigr registration)
        {
            var client = new PeerSigrClient(_hub, registration, connectionId, _responseAwaiter);
            RegistrationRequest registerationRequest = registration;
            //TODO: Change this to something unique and different then connection id
            registerationRequest.Properties.Add(CommonConstants.SigrReferenceTag, connectionId);
            var result = MasterHandle.Register(registerationRequest).GetAwaiter().GetResult();
            if (result.Status != "Success")
                throw new Exception("Registration failed");
            registration.RegistrationId = result.RegistrationId;
            //_hub.Clients
            //    .Client(connectionId)
            //    .SendAsync(nameof(SendRegisterResponse),
            //    new RegistrationResponseSigr() { RegistrationId = request.RegistrationId, Status = request. });
            _clients.Add(connectionId, client);
            return client;
        }

        public void RemoveClient(string connectionId)
        {
            PeerSigrClient sigrPeerClient = (PeerSigrClient)_clients[connectionId];
            _clients.Remove(connectionId);
            RegistrationRequest registerationRequest = sigrPeerClient.Registration;
            var result = MasterHandle.Unregister(registerationRequest).GetAwaiter().GetResult();
            if (result.Status != "Success")
                throw new Exception("Unregistration failed");
        }
    }
}
