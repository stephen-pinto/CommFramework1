using EasyRpc.Plugin.SignalR.Types;

namespace EasyRpc.Plugin.SignalR.Interfaces
{
    public interface IEasyRpcSignalRHub
    {
        Task Register(RegistrationRequestSigr request);
        Task SendRegisterResponse(RegistrationResponseSigr response);
        Task Unregister(RegistrationRequestSigr request);
        Task SendUnregisterResponse(RegistrationResponseSigr response);
        Task MakeRequest(MessageSigr message);
        Task SendMakeRequestResponse(MessageSigr message);
        Task Notify(MessageSigr message);
    }
}
