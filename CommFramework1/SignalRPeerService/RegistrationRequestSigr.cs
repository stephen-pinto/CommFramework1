namespace SignalRPeerService
{
    public class RegisterationRequestSigr
    {
        public string RegistrationId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }

    public class RegisterationResponseSigr
    {
        public string RegistrationId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
