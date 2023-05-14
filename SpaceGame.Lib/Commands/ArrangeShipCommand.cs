namespace SpaceGame.Lib;
using Vector;
using Hwdtech;

public class ArrangeShipCommand : ICommand  // "Game.Arrange.Ship"
{
    private IUObject _gameUObject;
    private IEnumerator<Vector> _positionEnumerator;

    public ArrangeShipCommand(IUObject gameUObject, IEnumerator<Vector> positionEnumerator)
    {
        _gameUObject = gameUObject;
        _positionEnumerator = positionEnumerator;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.UObject.SetProperty", _gameUObject, "Position", _positionEnumerator.Current).Execute();
        IoC.Resolve<ICommand>("Game.UObject.SetProperty", _gameUObject, "Fuel", 100.0).Execute();
        _positionEnumerator.MoveNext();
    }
}
