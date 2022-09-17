using System;

namespace SpaceGame
{
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

    interface IChangeableSpeed : IMovable
    {
        void setSpeed(double[] speed_vector);
    }

    interface IRotatable
    {
        double getAngle();
        void setAngle(double angle);
        double getAngularSpeed();
    }

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

    class Movement
    {
        public static void Move(IMovable obj)
        {

            if (obj.getCoords().Length == 0)
            {
                throw new ArgumentException("No coordinates");
            }
            if (obj.getSpeed().Length == 0)
            {
                throw new ArgumentException("No speed vector");
            }
            obj.setCoords(obj.getCoords().Zip(obj.getSpeed(), (a, b) => a + b).ToArray());  // a + b не округляется

        }
    }

    class Rotation
    {
        public static void Rotate(IRotatable obj)
        {
            obj.setAngle((obj.getAngle() + obj.getAngularSpeed()) % (2 * Math.PI));  // в радианах
            //! Q: кол-во направлений?
            // int maxDirections = 12;
            // int curDirection = (obj.getAngle()) / (360 / maxDirections);
            if (obj is IChangeableSpeed)
            {
                double modSpeed = Math.Sqrt(((IMovable)obj).getSpeed().Select(xi => xi * xi).Sum());
                ((IChangeableSpeed)obj).setSpeed(new double[] { Math.Round(modSpeed * Math.Cos(obj.getAngle())), Math.Round(modSpeed * Math.Sin(obj.getAngle())) });
            }
        }
    }

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
}
