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
        int getAngle();
        void setAngle(int angle);
        int getAngularSpeed();
    }

    public class Ship : IChangeableSpeed, IRotatable
    {
        double[] _coords;
        double[] _speed_vector;
        int _angle;
        int _angularSpeed;

        //! Q: поворот для 2-мерного?
        public Ship(double[] coords, double[] speed_vector, int angularSpeed = 45)
        {
            //! Q: МБ В КОНСТРУКТОРЕ?
            // if (coords.Length != speed_vector.Length)
            // {
            //     throw new ArgumentException("Не сопадают размерности векторов coords и speed_vector");
            // }
            _coords = coords;
            _speed_vector = speed_vector;
            _angle = (int)(Math.Atan2(_speed_vector[1], _speed_vector[0]) * (180 / Math.PI));
            _angularSpeed = angularSpeed;
            Console.WriteLine($"Created a Ship at ({String.Join(", ", _coords)}) with ({String.Join(", ", _speed_vector)}) Speed vector.\n" +
                              $"Angle: {_angle},\tAngular Speed: {_angularSpeed}.\n");
        }
        public Ship(double[] coords, int angle, double norma, int angularSpeed = 45)
        {
            _coords = coords;
            _angle = angle;
            _speed_vector = new double[] { norma * Math.Cos(angle), norma * Math.Sin(angle) };
            _angularSpeed = angularSpeed;
            Console.WriteLine($"Created a Ship at ({String.Join(", ", _coords)}) with ({String.Join(", ", _speed_vector)}) Speed vector.\n" +
                              $"Angle: {_angle},\tAngular Speed: {_angularSpeed}.\n");
        }
        public double[] getCoords() => _coords;
        public void setCoords(double[] coords) => _coords = coords;
        public double[] getSpeed() => _speed_vector;
        public void setSpeed(double[] speed_vector) => _speed_vector = speed_vector;
        public int getAngle() => _angle;
        public void setAngle(int angle) => _angle = angle;
        public int getAngularSpeed() => _angularSpeed;
        public void Print()
        {
            Console.WriteLine($"({String.Join(", ", _coords)}) with ({String.Join(", ", _speed_vector)}) Speed vector.\n" +
                              $"Angle: {_angle},\tAngular Speed: {_angularSpeed}.\n");
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
            obj.setCoords(obj.getCoords().Zip(obj.getSpeed(), (a, b) => a + b).ToArray());

        }
    }

    class Rotation
    {
        public static void Rotate(IRotatable obj)
        {
            obj.setAngle((obj.getAngle() + obj.getAngularSpeed()) % 360);
            //! Q: кол-во направлений?
            // int maxDirections = 12;
            // int curDirection = (obj.getAngle()) / (360 / maxDirections);
            if (obj is IChangeableSpeed)
            {
                double modSpeed = Math.Sqrt(((IMovable)obj).getSpeed().Select(xi => xi * xi).Sum());
                ((IChangeableSpeed)obj).setSpeed(new double[] { modSpeed * Math.Cos(obj.getAngle()), modSpeed * Math.Sin(obj.getAngle()) });
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Ship ship = new Ship(new double[] {2,5 }, new double[] { -7, 3 });
            Movement.Move(ship);
            ship.Print();
            Movement.Move(ship);
            ship.Print();
            Movement.Move(ship);
            ship.Print();

            Rotation.Rotate(ship);
            ship.Print();
            Movement.Move(ship);
            ship.Print();

            Rotation.Rotate(ship);
            ship.Print();
            Movement.Move(ship);
            ship.Print();

            Rotation.Rotate(ship);
            ship.Print();

        }
    }
}