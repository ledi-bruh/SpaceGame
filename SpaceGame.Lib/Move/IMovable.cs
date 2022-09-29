namespace SpaceGame.Lib;
using Vector;

public interface IMovable
{
    public Vector Position
    {
        get;
        set;
    }
    public Vector Velocity
    {
        get;
    }
}