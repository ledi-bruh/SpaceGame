namespace SpaceGame.Lib;
using Hwdtech;

public class ArrangeShipsCommand : ICommand  // "Game.Arrange.Ships"
{
    private IEnumerable<IUObject> _gameUObjects;

    public ArrangeShipsCommand(IEnumerable<IUObject> gameUObjects) { _gameUObjects = gameUObjects; }

    public void Execute()
    {
        var positionIterator = IoC.Resolve<IEnumerator<object>>("Game.Iterator.Position");

        _gameUObjects.ToList().ForEach(ship => IoC.Resolve<ICommand>("Game.Arrange.Ship", ship, positionIterator).Execute());

        positionIterator.Reset();
    }
}
