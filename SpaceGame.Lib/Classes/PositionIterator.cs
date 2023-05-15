namespace SpaceGame.Lib;
using Vector;
using Hwdtech;

public class PositionIterator : IEnumerator<object>  // "Game.Iterator.Position"
{
    private IList<Vector> _positions;
    private int _positionIndex;

    public PositionIterator()
    {
        _positionIndex = 0;
        _positions = IoC.Resolve<List<Vector>>("Game.Positions");
    }

    public object Current => _positions[_positionIndex];

    public bool MoveNext() => ++_positionIndex < _positions.Count;

    public void Reset() => _positionIndex = 0;

    public void Dispose() => throw new NotImplementedException();
}
