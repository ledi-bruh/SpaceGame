namespace SpaceGame.Lib;
using Hwdtech;

public class DeleteGameCommand : ICommand
{
    private string _gameId;

    public DeleteGameCommand(string gameId) => _gameId = gameId;

    public void Execute()
    {
        var gameMap = IoC.Resolve<IDictionary<string, IInjectable>>("Game.Map");
        gameMap[_gameId].Inject(
            IoC.Resolve<ICommand>("Game.Command.Empty")
        );

        var game_scope_map = IoC.Resolve<IDictionary<string, object>>("Game.Scope.Map");
        game_scope_map.Remove(_gameId);
    }
}
