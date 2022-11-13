namespace SpaceGame.Lib;

public class MoveCommand : ICommand
{
    private readonly IMovable _movable;

    public MoveCommand(IMovable obj) => _movable = obj;

    public void Execute() => this._movable.Position += this._movable.Velocity;
}
