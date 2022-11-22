namespace SpaceGame.Lib;
using Vector;

public interface IMoveStartable
{
    public IUObject Target { get; }
    public Vector Velocity { get; }
}
