namespace SignalRPeerService.Old
{
    public delegate Task<RegisterationResponseSigr> RegistererDelegate(RegisterationRequestSigr request);
    public delegate Task<MessageSigr> RequestDelegate(MessageSigr message);

    public interface IPeerClientSigr
    {
        //Task MakeRequest(MessageSigr message);
        //Task Notify(MessageSigr message);
    }
}
