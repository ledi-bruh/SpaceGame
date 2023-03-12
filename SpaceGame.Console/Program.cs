namespace SpaceGame.Console;
using System;
using Hwdtech;

class Program
{
    static void Main(string[] args)
    {
        int threadCount = int.Parse(args[0]);

        Console.WriteLine("Starting server...");
        IoC.Resolve<ICommand>("Server.Start", threadCount).Execute();
        Console.WriteLine("Server started successfully");

        Console.ReadKey();

        Console.WriteLine("Stopping server...");
        IoC.Resolve<ICommand>("Server.Stop.Soft").Execute();
        Console.WriteLine("Server stopped successfully");
    }
}
