using EasyRpc.Core.Client;
using EasyRpc.Types;
using Grpc.Core;

namespace EasyRpc.Master
{
    public class EasyRpcMasterService : MasterService.MasterServiceBase
    {
        private readonly RegisterDelegate _registerHandler;
        private readonly UnregisterDelegate _unregisterHandler;
        private readonly NotifyDelegate _notificationHandler;

        public EasyRpcMasterService(RegisterDelegate register, UnregisterDelegate unregister, NotifyDelegate notify)
        {
            _registerHandler = register;
            _unregisterHandler = unregister;
            _notificationHandler = notify;
        }

        public override async Task<RegistrationResponse> Register(RegistrationRequest request, ServerCallContext context)
        {
            return await _registerHandler(request);
        }

        public override async Task<RegistrationResponse> Unregister(RegistrationRequest request, ServerCallContext context)
        {
            return await _unregisterHandler(request);
        }

        public override async Task<Empty> Notify(Message message, ServerCallContext context)
        {
            _notificationHandler(message);
            return await Task.FromResult(new Empty());
        }
    }
}
