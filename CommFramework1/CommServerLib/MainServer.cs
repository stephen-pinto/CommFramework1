using CommServer;
using Grpc.Core;

namespace CommServerLib
{
    public class MainServer : CommService.CommServiceBase
    {
        public override Task<RegisterationResponse> Register(RegisterationRequest request, ServerCallContext context)
        {
            return base.Register(request, context);
        }

        public override Task<RegisterationResponse> Unregister(RegisterationRequest request, ServerCallContext context)
        {
            return base.Unregister(request, context);
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
