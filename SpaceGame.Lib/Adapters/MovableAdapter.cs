namespace SpaceGame.Lib;
using Vector;

public class MovableAdapter : IMovable
{
    private IUObject _uObject;

    public MovableAdapter(IUObject uObject) => _uObject = uObject;

    public Vector Position
    {
        get => (Vector)_uObject.GetProperty("Position");
        set => _uObject.SetProperty("Position", value);
    }

    public Vector Velocity
    {
        get => (Vector)_uObject.GetProperty("Velocity");
    }
}
