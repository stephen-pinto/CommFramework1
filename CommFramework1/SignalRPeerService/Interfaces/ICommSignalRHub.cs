using SignalRPeerService.Types;

namespace SignalRPeerService.Interfaces
{
    public interface ICommSignalRHub
    {
        Task Register(RegisterationRequestSigr request);
        Task SendRegisterResponse(RegisterationResponseSigr response);
        Task Unregister(RegisterationRequestSigr request);
        Task SendUnregisterResponse(RegisterationResponseSigr response);
        Task MakeRequest(MessageSigr message);
        Task SendMakeRequestResponse(MessageSigr message);
        Task Notify(MessageSigr message);
    }
}
