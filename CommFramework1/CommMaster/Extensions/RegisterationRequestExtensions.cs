using CommServices.CommMaster;
using CommMaster.PeerClient;

namespace CommMaster.Extensions
{
    internal static class RegisterationRequestExtensions
    {
        public static Peer ToPeer(this RegisterationRequest request)
        {
            return new Peer(
                request.RegistrationId,
                request.Name,
                request.Type,
                request.Address,
                request.Properties.ToDictionary(),
                DateTime.Now);
        }
    }
}
