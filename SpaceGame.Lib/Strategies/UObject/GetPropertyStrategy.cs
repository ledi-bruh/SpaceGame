namespace SpaceGame.Lib;

public class GetPropertyStrategy : IStrategy  // "Game.UObject.GetProperty"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];
        string name = (string)args[1];

        return uObject.GetProperty(name);
    }
}
