using System;

// interface IEntity
// {
//     double[] Coords { get; }
// }
interface IMovable
{
    double[] getCoords();
    void setCoords(double[] coords);
    double[] getSpeed();
}
interface IRotatable
{
    int getAngle();
    void setAngle(int angle);
}

public class Ship : IMovable
{
    double[] _coords, _speed_vector;

    public Ship()
    {
        _coords = new double[] { 0, 0 };
        _speed_vector = new double[] { 0, 0 };
        Console.WriteLine($"Created a Ship at ({String.Join(", ", _coords)}) with ({String.Join(", ", _speed_vector)}) Speed vector.");
    }
    public Ship(double[] coords, double[] speed_vector)
    {
        if (coords.Length != speed_vector.Length)
        {
            throw new ArgumentException("Не сопадают размерности веторов coords и speed_vector");
        }
        _coords = coords;
        _speed_vector = speed_vector;
        Console.WriteLine($"Created a Ship at ({String.Join(", ", _coords)}) with ({String.Join(", ", _speed_vector)}) Speed vector.");
    }
    public double[] getCoords() => _coords;
    public void setCoords(double[] coords) => _coords = coords;
    public double[] getSpeed() => _speed_vector;
    public void Print()
    {
        Console.WriteLine($"({String.Join(", ", _coords)})");
    }
}

class Movement
{
    public static void Move(IMovable obj) => obj.setCoords(obj.getCoords().Zip(obj.getSpeed(), (a, b) => a + b).ToArray());
}

class Rotation
{
    public static void Rotate(IRotatable obj)
    {
        
    }
}

class Program
{
    static void Main()
    {
        Ship ship0 = new Ship();
        ship0.Print();
        Ship ship = new Ship(new double[] { 12, 5, 1 }, new double[] { -7, 3, -1 });
        Movement.Move(ship);
        ship.Print();
        Movement.Move(ship);
        ship.Print();
        Movement.Move(ship);
        ship.Print();
    }
}
