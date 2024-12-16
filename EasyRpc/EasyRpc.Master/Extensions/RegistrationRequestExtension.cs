using EasyRpc.Core.Client;

namespace EasyRpc.Master.Extensions
{
    internal static class RegistrationRequestExtension
    {
        public static PeerInfo ToPeer(this RegistrationRequest request)
        {
            return new PeerInfo(
                request.RegistrationId,
                request.Name,
                request.Type,
                request.Address,
                request.Properties.ToDictionary(),
                DateTime.Now);
        }
    }
}
