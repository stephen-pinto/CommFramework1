using EasyRpc.Master;

namespace EasyRpc.Core.Server
{
    public interface IMasterService
    {
        Task<RegistrationResponse> Register(RegistrationRequest request);
        Task<RegistrationResponse> Unregister(RegistrationRequest request);
    }
}
