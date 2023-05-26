namespace SpaceGame.Lib;

public class GameAdapterRegisterStrategy : IStrategy  // "Game.Adapter.Register"
{
    public object Invoke(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var targetType = (Type)args[1];

        return new GameAdapterRegisterCommand(uObject, targetType);
    }
}
