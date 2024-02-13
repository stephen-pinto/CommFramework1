// See https://aka.ms/new-console-template for more information
using CommMaster;

Console.WriteLine("Welcome to Master service!");
CommService service = new CommService();
service.Start();

Console.WriteLine("Press any key to stop the service...");
Console.ReadKey();

service.Stop();