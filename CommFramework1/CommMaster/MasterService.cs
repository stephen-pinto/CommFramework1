using CommPeerServices.Base.Client;
using CommServices.CommMaster;
using Grpc.Core;

namespace CommMaster
{
    public class MasterService : CommMasterService.CommMasterServiceBase
    {
        private readonly RegisterDelegate _registerHandler;
        private readonly UnregisterDelegate _unregisterHandler;

        public MasterService(RegisterDelegate register, UnregisterDelegate unregister)
        {
            _registerHandler = register;
            _unregisterHandler = unregister;
        }

        public override async Task<RegisterationResponse> Register(RegisterationRequest request, ServerCallContext context)
        {
            return await _registerHandler(request);
        }

        public override async Task<RegisterationResponse> Unregister(RegisterationRequest request, ServerCallContext context)
        {
            return await _unregisterHandler(request);
        }
    }
}
