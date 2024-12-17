using Microsoft.AspNetCore.SignalR.Client;

namespace SigRTestClient
{
    public record MessageSigr(
            string To,
            string From,
            string Id,
            string Type,
            string Data,
            Dictionary<string, string> Metadata,
            Dictionary<string, string> Headers);

    public class RegistrationRequestSigr
    {
        public string? RegistrationId { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }

    public class RegistrationResponseSigr
    {
        public string? RegistrationId { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    internal class TestClient
    {
        internal void Run()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:55155/peer")
                .Build();

            connection.On<MessageSigr>("MakeRequest", (message) =>
            {
                Console.WriteLine($"[HUB]: {message}");
                connection.InvokeAsync("SendMakeRequestResponse", new MessageSigr() { From =  });
            });

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
