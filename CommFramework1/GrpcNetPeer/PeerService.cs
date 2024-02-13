using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;
using System.Diagnostics;

namespace GrpcNetPeer
{
    public class PeerService : CommPeerService.CommPeerServiceBase
    {
        public PeerService()
        {

        }

        public override Task<Message> MakeRequest(Message request, ServerCallContext context)
        {
            Debug.WriteLine($"Received request from {request.From} to {request.To} with body = {request.Data}");
            return Task.FromResult(new Message
            {
                From = "Peer",
                To = request.From,
                Data = "Successfully received request: " + request.Data
            });
        }

        public override Task<Message> Notify(Message request, ServerCallContext context)
        {
            Debug.WriteLine($"Received request from {request.From} to {request.To} with body = {request.Data}");

            return Task.FromResult(new Message
            {
                From = "Peer",
                To = request.From,
                Data = "Successfully received notification: " + request.Data
            });
        }
    }
}
