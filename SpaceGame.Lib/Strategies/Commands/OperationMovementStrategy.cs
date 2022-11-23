namespace SpaceGame.Lib;

public class OperationMovementStrategy : IStrategy  // "Game.Operation.Movement"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];

        List<ICommand> commands = new List<ICommand> { };

        IoC.Resolve<List<string>>("Config.Properties").ForEach(commandName =>
            commands.Add(IoC.Resolve<ICommand>(commandName, uObject))
        );

        ICommand macroCommand = IoC.Resolve<ICommand>("Game.Command.Macro", commands);

        IoC.Resolve<ICommand>("Game.UObject.SetProperty", uObject, "MovementCommand", macroCommand).Execute();

        ICommand command = IoC.Resolve<ICommand>("Game.Command.Getter", uObject, "MovementCommand");

        commands.Add(IoC.Resolve<ICommand>("Game.Command.Repeat", command));

        return command;
    }
}
