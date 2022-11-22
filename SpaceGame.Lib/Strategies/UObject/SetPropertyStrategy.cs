namespace SpaceGame.Lib;

public class SetPropertyStrategy : IStrategy  // "Game.UObject.SetProperty"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];
        string name = (string)args[1];
        object value = (object)args[2];

        return new SetPropertyCommand(uObject, name, value);
    }
}
