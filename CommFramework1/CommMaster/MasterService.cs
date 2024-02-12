using CommMaster.ClientManagement;
using Grpc.Core;

namespace CommMaster
{
    internal class MasterService : CommService.CommServiceBase
    {
        IClientRegistry _registry;

        internal MasterService(IClientRegistry registry)
        {
            _registry = registry;
        }

        public override Task<RegisterationResponse> Register(RegisterationRequest request, ServerCallContext context)
        {
            request.ClientId = Guid.NewGuid().ToString();
            _registry.Add(Guid.NewGuid().ToString(), request.ToClient());
            return new Task<RegisterationResponse>(() => new RegisterationResponse { ClientId = request.ClientId, Status = "Success" });
        }

        public override Task<RegisterationResponse> Unregister(RegisterationRequest request, ServerCallContext context)
        {
            _registry.Remove(request.ClientId);
            return new Task<RegisterationResponse>(() => new RegisterationResponse { ClientId = request.ClientId, Status = "Success" });
        }

        public override Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            return base.MakeRequest(request, context);
        }

        public override Task<Message> Notify(Message request, ServerCallContext context)
        {
            return base.Notify(request, context);
        }
    }
}
