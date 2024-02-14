namespace SignalRPeerService
{
    public enum MessageTypeSigr
    {
        NONE,
        REQUEST,
        RESPONSE,
        NOTIFICATION,
        ERROR
    }

    public record MessageSigr(
        string To,
        string From,
        string Id,
        string Type,
        string Data,
        Dictionary<string, string> Metadata,
        Dictionary<string, string> Headers);
}
