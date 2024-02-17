using CommPeerServices.Base.Client;
using CommServices.CommMaster;
using SignalRPeerService.Interfaces;
using SignalRPeerService.Types;

namespace SignalRPeerService
{
    public class DefaultSigrPeerClientFactory : ISigrPeerClientFactory
    {
        private readonly IMasterClient _masterClient;
        private readonly PeerHub _hub;
        private readonly ResponseAwaiter _responseAwaiter;
        private readonly Dictionary<string, IPeerClient> _clients;

        public DefaultSigrPeerClientFactory(
            PeerHub hub,
            IMasterClient masterClient,
            ResponseAwaiter responseAwaiter)
        {
            _hub = hub;
            _masterClient = masterClient;
            _responseAwaiter = responseAwaiter;
            _clients = new Dictionary<string, IPeerClient>();
        }

        public IPeerClient GetClient(string connectionId)
        {
            return _clients[connectionId];
        }

        public IPeerClient AddNewRegisteredClient(string connectionId, RegisterationRequestSigr registration)
        {
            var client = new SigrPeerClient(_hub, registration, connectionId, _responseAwaiter);
            RegisterationRequest registerationRequest = registration;
            //TODO: Change this to something unique and different then connection id
            registerationRequest.Properties.Add(CommonConstants.SigrReferenceTag, connectionId);
            var result = _masterClient.Register(registerationRequest).GetAwaiter().GetResult();
            if (result.Status != "Success")
                throw new Exception("Registration failed");
            registration.RegistrationId = result.RegistrationId;
            _clients.Add(connectionId, client);
            return client;
        }

        public void RemoveClient(string connectionId)
        {
            SigrPeerClient sigrPeerClient = (SigrPeerClient)_clients[connectionId];
            _clients.Remove(connectionId);
            RegisterationRequest registerationRequest = sigrPeerClient.Registration;
            var result = _masterClient.Unregister(registerationRequest).GetAwaiter().GetResult();
            if (result.Status != "Success")
                throw new Exception("Unregistration failed");
        }
    }
}
