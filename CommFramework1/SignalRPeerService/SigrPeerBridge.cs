using CommPeerServices.Base.Client;
using Microsoft.Extensions.DependencyInjection;
using SignalRPeerService.Interfaces;
using SignalRPeerService.Types;

namespace SignalRPeerService
{
    public class SigrPeerBridge
    {
        private readonly PeerHub _peerHub;
        private readonly ISigrPeerClientStore _sigrPeerClientFactory;
        private readonly IPeerClient _mainPeerClient;

        public SigrPeerBridge(IServiceProvider serviceProvider)
        {
            _peerHub = serviceProvider.GetService<PeerHub>() ?? throw new TypeInitializationException("PeerHub not initialized", null);
            _peerHub!.SetupHandlers(RegisterHandler, UnregisterHandler, MakeRequestHandler, NotifyHandler);

            _mainPeerClient = serviceProvider.GetService<IPeerClient>() ?? throw new TypeInitializationException("IPeerClient not initialized", null);
            _sigrPeerClientFactory = serviceProvider.GetService<ISigrPeerClientStore>() ?? throw new TypeInitializationException("ISigrPeerClientStore not initialized", null);
        }

        private Task RegisterHandler(string connectionId, RegisterationRequestSigr request)
        {
            _sigrPeerClientFactory.AddNewRegisteredClient(connectionId, request);
            return Task.CompletedTask;
        }

        private Task UnregisterHandler(string connectionId, RegisterationRequestSigr request)
        {
            _sigrPeerClientFactory.RemoveClient(connectionId);
            return Task.CompletedTask;
        }

        private async Task MakeRequestHandler(string connectionId, MessageSigr message)
        {
            await _mainPeerClient!.MakeRequest(message);
        }

        private async Task NotifyHandler(string connectionId, MessageSigr message)
        {
            await _mainPeerClient!.Notify(message);
        }
    }
}
