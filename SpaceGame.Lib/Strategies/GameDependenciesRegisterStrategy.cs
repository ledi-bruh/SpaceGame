namespace SpaceGame.Lib;


public class GameDependenciesRegisterStrategy : IStrategy //Server.Thread.Game.Register.Dependencies
{
    public object Invoke(params object[] args)
    {
        var gameID = (int)args[0];

        return new GameDependenciesRegisterCommand(gameID);
    }
}
