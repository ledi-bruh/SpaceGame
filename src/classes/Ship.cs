public class Ship : IChangeableSpeed, IRotatable
{
    double[] _coords;
    double[] _speed_vector;
    double _angle;
    double _angularSpeed;

    //! Q: поворот для 2-мерного?
    public Ship(double[] coords, double[] speed_vector, double angularSpeed = Math.PI / 4)
    {
        //! Q: МБ В КОНСТРУКТОРЕ?
        // if (coords.Length != speed_vector.Length)
        // {
        //     throw new ArgumentException("Не сопадают размерности векторов coords и speed_vector");
        // }
        _coords = coords;
        _speed_vector = speed_vector;
        _angle = Math.Atan2(_speed_vector[1], _speed_vector[0]);
        _angularSpeed = angularSpeed;
        Console.WriteLine($"Position: ({String.Join(", ", _coords)})\tSpeed vector: ({String.Join(", ", _speed_vector)})\n" +
                          $"Angle: {_angle}\tAngular Speed: {_angularSpeed}\n");
    }
    public Ship(double[] coords, double angle, double norma, double angularSpeed = Math.PI / 4)
    {
        _coords = coords;
        //! Q: угол [0:2pi]?
        _angle = angle;
        _speed_vector = new double[] { Math.Round(norma * Math.Cos(angle)), Math.Round(norma * Math.Sin(angle)) };
        _angularSpeed = angularSpeed;
        Console.WriteLine($"Position: ({String.Join(", ", _coords)})\tSpeed vector: ({String.Join(", ", _speed_vector)})\n" +
                          $"Angle: {_angle}\tAngular Speed: {_angularSpeed}\n");
    }
    public double[] getCoords() => _coords;
    public void setCoords(double[] coords) => _coords = coords;
    public double[] getSpeed() => _speed_vector;
    public void setSpeed(double[] speed_vector) => _speed_vector = speed_vector;
    public double getAngle() => _angle;
    public void setAngle(double angle) => _angle = angle;
    public double getAngularSpeed() => _angularSpeed;
    public void Print()
    {
        Console.WriteLine($"Position: ({String.Join(", ", _coords)})\tSpeed vector: ({String.Join(", ", _speed_vector)})\n" +
                          $"Angle: {_angle}\tAngular Speed: {_angularSpeed}\n");
    }
}
