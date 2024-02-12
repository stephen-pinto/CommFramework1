using CommMaster.PeerClient;
using CommServices.CommMaster;

namespace CommMaster.ClientManagement
{
    internal class PeerHandlerResolver
    {
        public IPeerHandle GetHandle(RegisterationRequest request)
        {
            switch (request.Type)
            {
                case "Local":
                    return null; //TODO: Replace with local handle
                case "Web":
                    return null; //TODO: Replace with socketio handle
                case "Grpc":
                default:
                    return new GrpcPeer(request.Address, new DefaultClientHandler());
            }
        }
    }
}
