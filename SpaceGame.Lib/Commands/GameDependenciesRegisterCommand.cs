using Hwdtech;

namespace SpaceGame.Lib;

public class GameDependenciesRegisterCommand : ICommand 
{
    private int _gameID;
    public GameDependenciesRegisterCommand(int gameID)
    {
        _gameID = gameID;
    }

    public void Execute()
    {
        var cmd = IoC.Resolve<ICommand>("Server.Thread.Game.Dependencies.Initialization");
        IoC.Resolve<ICommand>("Game.Queue.Push", _gameID, cmd).Execute();
    }
}
