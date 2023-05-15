namespace SpaceGame.Lib;


public interface IShootable
{
    IDictionary<string, int> Bullets
    {
        get;
        set;
    }
}
