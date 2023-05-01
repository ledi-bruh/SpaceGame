namespace SpaceGame.Lib;

public class DeleteGameStrategy : IStrategy  // "Game.Delete"
{
    public object Invoke(params object[] args)
    {
        string gameId = (string)args[0];

        return new DeleteGameCommand(gameId);
    }
}
