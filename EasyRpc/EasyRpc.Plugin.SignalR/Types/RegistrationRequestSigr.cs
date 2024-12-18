using EasyRpc.Master;

namespace EasyRpc.Plugin.SignalR.Types
{
    public record RegistrationRequestSigr(
        string RegistrationId,
        string Type,
        string Name)
    {
        public Dictionary<string, string> Properties { get; set; } = new();

        public static implicit operator RegistrationRequestSigr(RegistrationRequest registrationRequest)
        {
            var instance = new RegistrationRequestSigr(
                registrationRequest.RegistrationId,
                registrationRequest.Type,
                registrationRequest.Name);

            instance.Properties = registrationRequest.Properties != null ? new Dictionary<string, string>(registrationRequest.Properties) : new Dictionary<string, string>();
            return instance;
        }

        public static implicit operator RegistrationRequest(RegistrationRequestSigr registrationRequestSigr)
        {
            var instance = new RegistrationRequest
            {
                RegistrationId = registrationRequestSigr.RegistrationId,
                Type = registrationRequestSigr.Type,
                Name = registrationRequestSigr.Name
            };

            instance.Properties.MergeFrom(registrationRequestSigr.Properties);
            return instance;
        }
    }

    public record RegistrationResponseSigr(
        string RegistrationId,
        string Status,
        string Message
        )
    {
        public static implicit operator RegistrationResponseSigr(RegistrationResponse registrationResponse)
        {
            return new RegistrationResponseSigr(
                registrationResponse.RegistrationId,
                registrationResponse.Status,
                registrationResponse.Message);
        }

        public static implicit operator RegistrationResponse(RegistrationResponseSigr registrationResponseSigr)
        {
            return new RegistrationResponse
            {
                RegistrationId = registrationResponseSigr.RegistrationId,
                Status = registrationResponseSigr.Status,
                Message = registrationResponseSigr.Message
            };
        }
    }
}
