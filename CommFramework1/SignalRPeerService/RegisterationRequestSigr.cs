using CommServices.CommMaster;
using Google.Protobuf.Collections;

namespace SignalRPeerService
{
    public class RegisterationRequestSigr
    {
        public string RegistrationId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public static implicit operator RegisterationRequestSigr(RegisterationRequest registrationRequest)
        {
            return new RegisterationRequestSigr
            {
                RegistrationId = registrationRequest.RegistrationId,
                Type = registrationRequest.Type,
                Name = registrationRequest.Name,
                Properties = registrationRequest.Properties != null ? new Dictionary<string, string>(registrationRequest.Properties) : new Dictionary<string, string>()
            };
        }

        public static implicit operator RegisterationRequest(RegisterationRequestSigr registrationRequestSigr)
        {
            var request = new RegisterationRequest
            {
                RegistrationId = registrationRequestSigr.RegistrationId,
                Type = registrationRequestSigr.Type,
                Name = registrationRequestSigr.Name
            };

            request.Properties.MergeFrom(registrationRequestSigr.Properties);
            return request;
        }
    }

    public class RegisterationResponseSigr
    {
        public string RegistrationId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }

        public static implicit operator RegisterationResponseSigr(RegisterationResponse registrationResponse)
        {
            return new RegisterationResponseSigr
            {
                RegistrationId = registrationResponse.RegistrationId,
                Status = registrationResponse.Status,
                Message = registrationResponse.Message
            };
        }

        public static implicit operator RegisterationResponse(RegisterationResponseSigr registrationResponseSigr)
        {
            return new RegisterationResponse
            {
                RegistrationId = registrationResponseSigr.RegistrationId,
                Status = registrationResponseSigr.Status,
                Message = registrationResponseSigr.Message
            };
        }
    }
}
