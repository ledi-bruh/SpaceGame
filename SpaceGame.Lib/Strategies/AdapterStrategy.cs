namespace SpaceGame.Lib;

public class AdapterStrategy : IStrategy  // "Game.Adapter"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];
        Type adapterType = (Type)args[1];

        return Activator.CreateInstance(adapterType, uObject)!;
    }
}
