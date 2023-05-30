namespace SpaceGame.Lib;
using Vector;

public interface IShootable
{
    string AmmoType
    {
        get;
        set;
    }
    Vector projectilePosition
    {
        get;
        set;
    }
    Vector projectileVelocity
    {
        get;
        set;
    }
}
