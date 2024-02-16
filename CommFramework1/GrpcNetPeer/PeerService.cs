using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace GrpcNetPeer
{
    public class PeerService : CommPeerService.CommPeerServiceBase
    {
        public PeerService()
        {

        }

        public override Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            Console.WriteLine($"Received request from {request.From} to {request.To} with body = {request.Data}");
            
            return Task.FromResult(new Message
            {
                From = "Peer",
                To = request.From,
                Data = "Successfully received request: " + request.Data
            });
        }

        public override Task<Empty> Notify(Message request, ServerCallContext context)
        {
            Console.WriteLine($"Received request from {request.From} to {request.To} with body = {request.Data}");
            return Task.FromResult(new Empty());
        }
    }
}
