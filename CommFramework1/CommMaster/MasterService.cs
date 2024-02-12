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
            request.RegistrationId = Guid.NewGuid().ToString();
            PeerHandlerResolver resolver = new PeerHandlerResolver();
            var handle = resolver.GetHandle(request);
            _registry.Add(Guid.NewGuid().ToString(), new PeerRegistryEntry(request.RegistrationId, request.ToPeer(), handle));
            return new Task<RegisterationResponse>(() => new RegisterationResponse { RegistrationId = request.RegistrationId, Status = "Success" });
        }

        public override Task<RegisterationResponse> Unregister(RegisterationRequest request, ServerCallContext context)
        {
            _registry.Remove(request.RegistrationId);
            return new Task<RegisterationResponse>(() => new RegisterationResponse { RegistrationId = request.RegistrationId, Status = "Success" });
        }
    }
}
