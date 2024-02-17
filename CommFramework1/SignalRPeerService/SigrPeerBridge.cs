using CommPeerServices.Base.Client;
using Microsoft.Extensions.DependencyInjection;
using SignalRPeerService.Interfaces;
using SignalRPeerService.Types;

namespace SignalRPeerService
{
    public class SigrPeerBridge
    {
        private readonly PeerHub? _peerHub;
        private readonly ISigrPeerClientFactory _sigrPeerClientFactory;
        private readonly IMasterClient? _masterClient;
        private readonly IPeerClient? _mainPeerClient;

        public SigrPeerBridge(IServiceProvider serviceProvider)
        {
            _peerHub = serviceProvider.GetService<PeerHub>();
            _peerHub!.SetupHandlers(RegisterHandler, UnregisterHandler, MakeRequestHandler, NotifyHandler);
            var responseAwaiter = serviceProvider.GetService<ResponseAwaiter>();
            _masterClient = serviceProvider.GetService<IMasterClient>();
            _mainPeerClient = serviceProvider.GetService<IPeerClient>();
            _sigrPeerClientFactory = new DefaultSigrPeerClientFactory(
                _peerHub, 
                _masterClient!, 
                responseAwaiter!);
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
