namespace SignalRPeerService
{
    public interface ICommMasterClientSigr
    {
        Task Register(RegisterationRequestSigr request);
        Task Unregister(RegisterationRequestSigr request);
    }   
}
