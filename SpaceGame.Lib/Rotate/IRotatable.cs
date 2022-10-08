namespace SpaceGame.Lib;

public interface IRotatable
{
    public int Direction
    {
        get;
        set;
    }
    public int AngularVelocity
    {
        get;
    }
}