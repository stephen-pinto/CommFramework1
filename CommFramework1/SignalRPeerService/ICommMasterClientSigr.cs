namespace SignalRPeerService
{
    public interface ICommMasterClientSigr
    {
        Task<RegisterationResponseSigr> Register(RegisterationRequestSigr request);
        Task<RegisterationResponseSigr> Unregister(RegisterationRequestSigr request);
    }   
}
