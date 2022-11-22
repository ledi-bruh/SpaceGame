namespace SpaceGame.Lib;

public class CreateNewObjectStrategy : IStrategy  // "Game.CreateObject"
{
    public object Invoke(params object[] args)
    {
        return new UObject();
    }
}
