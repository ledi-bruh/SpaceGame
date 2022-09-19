public class Ship : IMovable, IRotatable
{
    double[] _coords;
    double _norma;
    double _angleDirection;
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
        _norma = Math.Sqrt(speed_vector.Select(x => x * x).Sum());
        _angleDirection = Math.Atan2(speed_vector[1], speed_vector[0]);
        _angularSpeed = angularSpeed;
        Console.WriteLine($"Position: ({String.Join(", ", _coords)})\tNorma: {_norma}\n" +
                          $"Angle direction: {_angleDirection}\tAngular Speed: {_angularSpeed}\n");
    }
    public Ship(double[] coords, double angleDirection, double norma, double angularSpeed = Math.PI / 4)
    {
        _coords = coords;
        //! Q: угол [0:2pi]?
        _angleDirection = angleDirection;
        _norma = norma;
        _angularSpeed = angularSpeed;
        Console.WriteLine($"Position: ({String.Join(", ", _coords)})\tNorma: {_norma}\n" +
                          $"Angle direction: {_angleDirection}\tAngular Speed: {_angularSpeed}\n");
    }
    public double[] getCoords() => _coords;
    public void setCoords(double[] coords) => _coords = coords;
    public double getNorma() => _norma;
    public double getAngleDirection() => _angleDirection;
    public void setAngleDirection(double angleDirection) => _angleDirection = angleDirection;
    public double getAngularSpeed() => _angularSpeed;
    public void Print()
    {
        Console.WriteLine($"Position: ({String.Join(", ", _coords)})\tNorma: {_norma}\n" +
                          $"Angle direction: {_angleDirection}\tAngular Speed: {_angularSpeed}\n");
    }
}
