namespace SpaceGame.Lib;

public interface IRotatable
{
    public int Direction  //! Vector Direction
    {
        get;
        set;
    }
    public int AngularVelocity
    {
        get;
    }
}