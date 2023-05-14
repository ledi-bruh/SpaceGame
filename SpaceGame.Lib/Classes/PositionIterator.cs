namespace SpaceGame.Lib;
using System.Collections;
using Vector;
using Hwdtech;
using System;

public class PositionIterator : IEnumerator<Vector>
{
    private IList<Vector> _positions;
    private int _positionIndex;

    public PositionIterator()
    {
        _positionIndex = 0;
        _positions = IoC.Resolve<List<Vector>>("Game.Positions");
    }

    public Vector Current => _positions[_positionIndex];
    object IEnumerator.Current => Current;

    public bool MoveNext() => ++_positionIndex < _positions.Count;

    public void Reset() => _positionIndex = 0;

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
