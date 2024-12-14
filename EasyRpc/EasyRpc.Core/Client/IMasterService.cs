using EasyRpc.Master;
using EasyRpc.Types;

namespace EasyRpc.Core.Client
{
    public delegate Task<RegistrationResponse> RegisterDelegate(RegistrationRequest request);
    public delegate Task<RegistrationResponse> UnregisterDelegate(RegistrationRequest request);
    public delegate Task<Empty> NotifyDelegate(Message message);

    public interface IMasterService : IDisposable
    {
        Task<RegistrationResponse> Register(RegistrationRequest request);

        Task<RegistrationResponse> Unregister(RegistrationRequest request);

        Task<Empty> Notify(Message message);
    }
}
