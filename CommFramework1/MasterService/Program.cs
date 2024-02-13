// See https://aka.ms/new-console-template for more information
using CommMaster;

Console.WriteLine("Welcome to Master service!");
CommService service = new CommService("localhost", 50051);
service.Start();

Console.WriteLine("Press any key to stop the service...");
Console.ReadKey();

service.Stop();