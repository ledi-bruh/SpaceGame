namespace SpaceGame.Console;
using System;
using Hwdtech;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting server...");
        IoC.Resolve<ICommand>("Server.Start", args[0]).Execute();
        Console.WriteLine("Server started successfully");

        Console.ReadKey();

        Console.WriteLine("Stopping server...");
        IoC.Resolve<ICommand>("Server.Stop.Soft").Execute();
        Console.WriteLine("Server stopped successfully");
    }
}
