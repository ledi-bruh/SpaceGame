using System;

interface IMovable
{
    double[] SpeedVector { get; }
    void Move();
}

interface IRotatable
{
    void Rotate();
}

interface IEntity
{
    double[] Coords { get; }
}

public class Ship : IEntity, IMovable, IRotatable
{
    double[] _coords;
    public double[] Coords
    {
        get => _coords;
    }

    double[] _speed_vector;
    public double[] SpeedVector
    {
        get => _speed_vector;
    }

    public Ship()
    {
        _coords = new double[] { 0, 0 };
        _speed_vector = new double[] { 0, 0 };
        Console.WriteLine($"Created a Ship in ({_coords[0].ToString()}, {_coords[1].ToString()}) with ({_speed_vector[0].ToString()}, {_speed_vector[1].ToString()}) Speed vector.");
    }


    public void Move()
    {
        Console.WriteLine("Move");
    }

    public void Rotate() { Console.WriteLine("Rotate"); }
}


class Program
{
    static void Main()
    {
        Ship ship = new Ship();
    }
}
