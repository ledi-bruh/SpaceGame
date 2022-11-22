namespace SpaceGame.Lib;

public class OperationMoveStrategy : IStrategy  // "Game.Operation.Move"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];

        IMovable movable = IoC.Resolve<IMovable>("Game.Adapter", uObject, typeof(MovableAdapter));

        List<ICommand> commands = new List<ICommand> { IoC.Resolve<ICommand>("Game.Command.Move", movable) };

        ICommand macroCommand = IoC.Resolve<ICommand>("Game.Command.Macro", commands);

        ICommand command = IoC.Resolve<ICommand>("Game.Command.Getter", uObject, "MoveCommand");

        commands.Add(IoC.Resolve<ICommand>("Game.Command.Repeat", command));

        IoC.Resolve<ICommand>("Game.UObject.SetProperty", uObject, "MoveCommand", macroCommand).Execute();

        return command;
    }
}
