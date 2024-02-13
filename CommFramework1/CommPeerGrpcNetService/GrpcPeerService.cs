﻿using CommServices.CommPeer;
using CommServices.CommShared;
using Grpc.Core;

namespace CommPeerGrpcNetService
{
    public class GrpcPeerService : CommPeerService.CommPeerServiceBase
    {
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

        public override Task<Message> Notify(Message request, ServerCallContext context)
        {
            Console.WriteLine($"Received request from {request.From} to {request.To} with body = {request.Data}");

            return Task.FromResult(new Message
            {
                From = "Peer",
                To = request.From,
                Data = "Successfully received notification: " + request.Data
            });
        }
    }
}