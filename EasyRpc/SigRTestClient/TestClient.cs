using Microsoft.AspNetCore.SignalR.Client;

namespace SigRTestClient
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

    internal class TestClient
    {
        private string _id = string.Empty;
        private string _type = "SignalRClient";
        private string _peerId = string.Empty;
        private RegistrationRequestSigr? _registration;

        internal void Run()
        {
            Thread.Sleep(2000);

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:55155/peer", opts =>
                {
                    opts.HttpMessageHandlerFactory = (handler) =>
                    {
                        if (handler is HttpClientHandler clientHandler)
                        {
                            // clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                        }
                        return handler;
                    };
                })
                .Build();

            connection.StartAsync();

            //Handle registration
            connection.On<RegistrationResponseSigr>(nameof(IEasyRpcSignalRHub.SendRegisterResponse), (message) =>
            {
                Console.WriteLine($"[HUB]: {message}");
                _id = message.RegistrationId;
            });

            _registration = new RegistrationRequestSigr("", _type, "SigRClient1");
            connection.InvokeAsync(nameof(IEasyRpcSignalRHub.Register), _registration);

            //Handle requests
            connection.On<MessageSigr>(nameof(IEasyRpcSignalRHub.MakeRequest), (message) =>
            {
                Console.WriteLine($"[HUB]: {message}");
                connection.InvokeAsync(nameof(IEasyRpcSignalRHub.SendMakeRequestResponse),
                    new MessageSigr(_peerId, _id,
                    message.Id, _type,
                    "This is some data from SigR Client/Peer for Request: " + message.Data));
            });

            //Handle close and reconnect
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            connection.InvokeAsync(nameof(IEasyRpcSignalRHub.Unregister), _registration);
            Console.WriteLine("Unregistered...");
        }
    }
}
