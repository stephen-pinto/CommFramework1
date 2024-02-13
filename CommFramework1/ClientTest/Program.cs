// See https://aka.ms/new-console-template for more information
using GrpcNetPeer;

Console.WriteLine("Welcome to Peer1 service!");
PeerNetServices services = new PeerNetServices();
services.Start("https://localhost:50051", "https://localhost:50052", "https://localhost:50055");

Console.WriteLine("Press any key to test message send");
Console.ReadKey();

services.MakeRequest("Hello from Peer1");

Console.WriteLine("Press any key to stop the service...");
Console.ReadKey();


services.Stop();