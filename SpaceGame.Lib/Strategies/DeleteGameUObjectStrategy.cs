namespace SpaceGame.Lib;

public class DeleteGameUObjectStrategy : IStrategy  // "Game.Delete.UObject"
{
    public object Invoke(params object[] args)
    {
        int uObjectId = (int)args[0];

        return new DeleteGameUObjectCommand(uObjectId);
    }
}
