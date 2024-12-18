using Microsoft.AspNetCore.SignalR.Client;

namespace SigRTestClient
{
    internal class TestClient
    {
        private string _id = string.Empty;
        private string _type = "SigR";
        private string _peerId = string.Empty;

        internal void Run()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:55155/peer")
                .Build();

            //Handle registration
            connection.On<RegistrationResponseSigr>("RegisterResponse", (message) =>
            {
                Console.WriteLine($"[HUB]: {message}");
                _id = message.RegistrationId;
            });
            connection.InvokeAsync(_peerId, new RegistrationRequestSigr(_peerId, _type, "SigRClient1"));

            //Handle requests
            connection.On<MessageSigr>("MakeRequest", (message) =>
            {
                Console.WriteLine($"[HUB]: {message}");
                connection.InvokeAsync("SendMakeRequestResponse",
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
        }
    }
}
