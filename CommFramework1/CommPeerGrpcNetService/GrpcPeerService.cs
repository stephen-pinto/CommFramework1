//using CommServices.CommPeer;
//using CommServices.CommShared;
//using Grpc.Core;

//namespace CommPeerGrpcNetService
//{
//    public delegate Task<Message> RemoteRequestDelegate(Message request, ServerCallContext context);

//    public class GrpcPeerService : CommPeerService.CommPeerServiceBase
//    {
//        private RemoteRequestDelegate _makeRequestHandler { get; }
//        private RemoteRequestDelegate _notifyHandler { get; }

//        public GrpcPeerService(RemoteRequestDelegate makeRequestHandler, RemoteRequestDelegate notifyHandler)
//        {
//            _makeRequestHandler = makeRequestHandler;
//            _notifyHandler = notifyHandler;
//        }

//        public override async Task<Message> MakeRequest(Message request, ServerCallContext context)
//        {
//            Console.WriteLine($"Received request from {request.From} to {request.To} with body = {request.Data}");
//            return await _makeRequestHandler(request, context).ConfigureAwait(false);
//        }

//        public override async Task<Message> Notify(Message request, ServerCallContext context)
//        {
//            Console.WriteLine($"Received request from {request.From} to {request.To} with body = {request.Data}");
//            return await _notifyHandler(request, context).ConfigureAwait(false);
//        }
//    }
//}
