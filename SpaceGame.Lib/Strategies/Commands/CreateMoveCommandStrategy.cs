namespace SpaceGame.Lib;

public class CreateMoveCommandStrategy : IStrategy  // "Game.Command.Move"
{
    public object Invoke(params object[] args)
    {
        IMovable movable = (IMovable)args[0];

        return new MoveCommand(movable);
    }
}
