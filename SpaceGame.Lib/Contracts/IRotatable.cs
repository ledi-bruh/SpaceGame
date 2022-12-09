namespace SpaceGame.Lib;
using Angle;

public interface IRotatable
{
    public Angle Direction { get; set; }
    public Angle AngularVelocity { get; }
}
