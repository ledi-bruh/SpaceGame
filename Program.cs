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
    double[] _coords;
    double[] _speed_vector;

    public Ship()
    {
        _coords = new double[] { 0, 0 };
        _speed_vector = new double[] { 0, 0 };
        Console.WriteLine($"Created a Ship at ({_coords[0].ToString()}, {_coords[1].ToString()}) with ({_speed_vector[0].ToString()}, {_speed_vector[1].ToString()}) Speed vector.");
    }
    public Ship(double[] coords, double[] speed_vector)
    {
        _coords = coords;
        _speed_vector = speed_vector;
        Console.WriteLine($"Created a Ship at ({_coords[0].ToString()}, {_coords[1].ToString()}) with ({_speed_vector[0].ToString()}, {_speed_vector[1].ToString()}) Speed vector.");
    }
    public double[] getCoords()
    {
        return _coords;
    }
    public void setCoords(double[] coords)
    {
        _coords = coords;
    }
    public double[] getSpeed()
    {
        return _speed_vector;
    }
    public void Print()
    {
        Console.WriteLine($"{_coords[0].ToString()}, {_coords[1].ToString()}");
    }
}

class Movement
{
    public static void Move(IMovable obj)
    {
        double[] coords = obj.getCoords();
        for (int i = 0; i < coords.Length; i++)
        {
            coords[i] += obj.getSpeed()[i];
        }
        obj.setCoords(coords);
    }
}


class Program
{
    static void Main()
    {
        Ship ship = new Ship(new double[] { 12, 5 }, new double[] { -7, 3 });
        Movement.Move(ship);
        ship.Print();
        Movement.Move(ship);
        ship.Print();
        Movement.Move(ship);
        ship.Print();
    }
}
