using EasyRpc.Core.Client;
using Grpc.Core;

namespace EasyRpc.Master
{
    public class EasyRpcMasterService : MasterService.MasterServiceBase
    {
        private readonly RegisterDelegate _registerHandler;
        private readonly UnregisterDelegate _unregisterHandler;

        public EasyRpcMasterService(RegisterDelegate register, UnregisterDelegate unregister)
        {
            _registerHandler = register;
            _unregisterHandler = unregister;
        }

        public override async Task<RegistrationResponse> Register(RegistrationRequest request, ServerCallContext context)
        {
            return await _registerHandler(request);
        }

        public override async Task<RegistrationResponse> Unregister(RegistrationRequest request, ServerCallContext context)
        {
            return await _unregisterHandler(request);
        }
    }
}
