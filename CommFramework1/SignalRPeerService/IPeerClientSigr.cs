namespace SignalRPeerService
{
    public interface IPeerClientSigr
    {
        Task<MessageSigr> MakeRequest(MessageSigr message);

        Task<MessageSigr> Notify(MessageSigr message);
    }
}
