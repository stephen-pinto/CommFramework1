using EasyRpc.Core.Base;
using EasyRpc.Master;

namespace EasyRpc.Core.Client
{
    public interface IPeerClientFactory
    {
        IPeerService GetHandle(RegistrationRequest registerationRequest);
    }
}
