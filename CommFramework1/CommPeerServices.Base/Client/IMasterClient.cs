using CommServices.CommMaster;
using Grpc.Core;

namespace CommPeerServices.Base.Client
{
    public delegate Task<RegisterationResponse> RegisterDelegate(RegisterationRequest request);
    public delegate Task<RegisterationResponse> UnregisterDelegate(RegisterationRequest request);

    public interface IMasterClient
    {
        Task<RegisterationResponse> Register(RegisterationRequest request);

        Task<RegisterationResponse> Unregister(RegisterationRequest request);
    }
}
