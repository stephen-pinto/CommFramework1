using EasyRpc.Master;

namespace EasyRpc.Core.Client
{
    public interface IPeerClientFactory
    {
        IPeerClient GetHandle(RegistrationRequest registerationRequest);
    }
}
