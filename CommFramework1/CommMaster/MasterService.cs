using CommMaster.ClientManagement;
using CommMaster.Extensions;
using CommServices.CommMaster;
using Grpc.Core;
using System.Diagnostics;

namespace CommMaster
{
    public class MasterService : CommMasterService.CommMasterServiceBase
    {
        private readonly PeerHandlerResolver _resolver;
        private readonly IPeerRegistry _registry;

        internal MasterService(PeerHandlerResolver resolver, IPeerRegistry registry)
        {
            _resolver = resolver;
            _registry = registry;
        }

        public override async Task<RegisterationResponse> Register(RegisterationRequest request, ServerCallContext context)
        {
            request.RegistrationId = Guid.NewGuid().ToString();
            
            var handle = _resolver.GetHandle(request);

            _registry.Add(Guid.NewGuid().ToString(), new PeerRegistryEntry(request.RegistrationId, request.ToPeer(), handle));

            Debug.WriteLine($"Registered peer {request.Name} with id {request.RegistrationId}");

            //FIXME: Return another id as regid beacause it can be misidentified by another client.
            //Possibly try to use jwt token for registration id which can also be used for secure communication.
            return await Task.FromResult(new RegisterationResponse
            {
                RegistrationId = request.RegistrationId,
                Status = "Success"
            });
        }

        public override async Task<RegisterationResponse> Unregister(RegisterationRequest request, ServerCallContext context)
        {
            _registry.Remove(request.RegistrationId);

            Debug.WriteLine($"Unregistered peer with id {request.RegistrationId}");

            return await Task.FromResult(new RegisterationResponse
            {
                RegistrationId = request.RegistrationId,
                Status = "Success"
            });
        }
    }
}
