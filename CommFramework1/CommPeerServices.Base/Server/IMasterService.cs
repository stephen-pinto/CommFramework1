using CommServices.CommMaster;

namespace CommPeerServices.Base.Server
{
    public interface IMasterService
    {
        Task<RegisterationResponse> Register(RegisterationRequest request);
        Task<RegisterationResponse> Unregister(RegisterationRequest request);
    }
}
