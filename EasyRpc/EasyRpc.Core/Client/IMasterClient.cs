using EasyRpc.Master;

namespace EasyRpc.Core.Client
{
    public delegate Task<RegistrationResponse> RegisterDelegate(RegistrationRequest request);
    public delegate Task<RegistrationResponse> UnregisterDelegate(RegistrationRequest request);

    public interface IMasterClient
    {
        Task<RegistrationResponse> Register(RegistrationRequest request);

        Task<RegistrationResponse> Unregister(RegistrationRequest request);
    }
}
