namespace SigRTestClient
{
    public record MessageSigr(
            string To,
            string From,
            string Id,
            string Type,
            string Data)
    {
        Dictionary<string, string> Metadata { get; set; } = new();

        Dictionary<string, string> Headers { get; set; } = new();
    }

    public record RegistrationRequestSigr(string RegistrationId, string Type, string Name)
    {
        Dictionary<string, string> Properties { get; set; } = new();
    }

    public record RegistrationResponseSigr(string RegistrationId, string Status, string Message);
}
