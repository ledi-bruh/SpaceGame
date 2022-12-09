namespace SpaceGame.Lib;

public class MoveCommand : ICommand
{
    private readonly IMovable _movable;

    public MoveCommand(IMovable movable) => _movable = movable;

    public void Execute() => _movable.Position += _movable.Velocity;
}
