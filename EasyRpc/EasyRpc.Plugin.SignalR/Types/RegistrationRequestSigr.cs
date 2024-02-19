using EasyRpc.Master;

namespace EasyRpc.Plugin.SignalR.Types
{
    public class RegistrationRequestSigr
    {
        public string? RegistrationId { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public RegistrationRequestSigr()
        {
            Properties = new Dictionary<string, string>();
        }

        public static implicit operator RegistrationRequestSigr(RegistrationRequest registrationRequest)
        {
            return new RegistrationRequestSigr
            {
                RegistrationId = registrationRequest.RegistrationId,
                Type = registrationRequest.Type,
                Name = registrationRequest.Name,
                Properties = registrationRequest.Properties != null ? new Dictionary<string, string>(registrationRequest.Properties) : new Dictionary<string, string>()
            };
        }

        public static implicit operator RegistrationRequest(RegistrationRequestSigr registrationRequestSigr)
        {
            var request = new RegistrationRequest
            {
                RegistrationId = registrationRequestSigr.RegistrationId,
                Type = registrationRequestSigr.Type,
                Name = registrationRequestSigr.Name
            };

            request.Properties.MergeFrom(registrationRequestSigr.Properties);
            return request;
        }
    }

    public class RegistrationResponseSigr
    {
        public string? RegistrationId { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }

        public static implicit operator RegistrationResponseSigr(RegistrationResponse registrationResponse)
        {
            return new RegistrationResponseSigr
            {
                RegistrationId = registrationResponse.RegistrationId,
                Status = registrationResponse.Status,
                Message = registrationResponse.Message
            };
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
