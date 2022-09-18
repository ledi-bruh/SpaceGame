namespace SpaceGame;
using System;

class Program
{
    static void Main()
    {
        // Ship ship = new Ship(new double[] { 5, 2 }, new double[] { 1, 0 });
        Ship ship = new Ship(new double[] { 5, 2 }, 0, 1);

        Movement.Move(ship);
        ship.Print();
        Console.WriteLine("- - - - -\n");

        for (int i = 0; i < 7; i++)
        {
            Rotation.Rotate(ship);
            ship.Print();
            Movement.Move(ship);
            ship.Print();
            Console.WriteLine("- - - - -\n");
        }

        Rotation.Rotate(ship);
        ship.Print();
    }
}
