// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


CommClientLib.CommClient client = new CommClientLib.CommClient();
client.Connect(50051, "Hello from ConsoleClient");
