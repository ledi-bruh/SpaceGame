namespace SpaceGame.Lib;
using Hwdtech;

public class CreateGameUObjectCollectionCommand : ICommand  // "Game.UObject.Collection.Create"
{
    private int _gameUObjectCount;

    public CreateGameUObjectCollectionCommand(int gameUObjectCount) => _gameUObjectCount = gameUObjectCount;

    public void Execute()
    {
        var gameUObjectMap = IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject.Map");

        Enumerable.Range(0, _gameUObjectCount).ToList().ForEach(
            i => gameUObjectMap.Add(i, IoC.Resolve<IUObject>("Game.UObject.Create"))
        );
    }
}
