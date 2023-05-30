namespace SpaceGame.Lib;
using Vector;
using Hwdtech;

public class ArrangeShipCommand : ICommand  // "Game.Arrange.Ship"
{
    private IUObject _gameUObject;
    private IEnumerator<object> _positionEnumerator;

    public ArrangeShipCommand(IUObject gameUObject, IEnumerator<object> positionEnumerator)
    {
        _gameUObject = gameUObject;
        _positionEnumerator = positionEnumerator;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.UObject.SetProperty", _gameUObject, "Position", _positionEnumerator.Current).Execute();
        _positionEnumerator.MoveNext();
    }
}
