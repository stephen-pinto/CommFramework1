using CommMaster.ClientManagement;
using CommMaster.Extensions;
using CommServices.CommMaster;
using Grpc.Core;

namespace CommMaster
{
    internal class MasterService : CommMasterService.CommMasterServiceBase
    {
        private IClientRegistry _registry;

        internal MasterService(IClientRegistry registry)
        {
            _registry = registry;
        }

        public override Task<RegisterationResponse> Register(RegisterationRequest request, ServerCallContext context)
        {
            request.ClientId = Guid.NewGuid().ToString();
            _registry.Add(Guid.NewGuid().ToString(), request.ToPeer());
            return new Task<RegisterationResponse>(() => new RegisterationResponse { ClientId = request.ClientId, Status = "Success" });
        }

        public override Task<RegisterationResponse> Unregister(RegisterationRequest request, ServerCallContext context)
        {
            _registry.Remove(request.ClientId);
            return new Task<RegisterationResponse>(() => new RegisterationResponse { ClientId = request.ClientId, Status = "Success" });
        }    
    }
}
