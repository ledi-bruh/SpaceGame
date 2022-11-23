namespace SpaceGame.Lib;

public class CreateMoveCommandStrategy : IStrategy  // "Game.Command.Move"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];

        return new MoveCommand(
            IoC.Resolve<IMovable>("Game.Adapter", uObject, typeof(MovableAdapter))
        );
    }
}
