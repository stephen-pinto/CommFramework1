namespace SignalRPeerService.Old
{
    public interface ICommMasterClientSigr
    {
        Task Register(RegisterationRequestSigr request);
        Task Unregister(RegisterationRequestSigr request);
    }
}
