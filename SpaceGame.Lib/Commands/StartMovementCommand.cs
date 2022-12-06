namespace SpaceGame.Lib;

public class StartMovementCommand : ICommand
{
    private IStartable _startable;

    public StartMovementCommand(IStartable startable) => _startable = startable;

    public void Execute()
    {
        _startable.Parameters.ToList().ForEach(parameter =>
            IoC.Resolve<ICommand>(
                "Game.UObject.SetProperty",
                _startable.Target,
                parameter.Key,
                parameter.Value
            ).Execute()
        );

        ICommand injectCommand = IoC.Resolve<ICommand>("Game.Operation.Movement", _startable.Target);

        IoC.Resolve<ICommand>("Game.UObject.SetProperty", _startable.Target, "Movement", injectCommand).Execute();

        IoC.Resolve<Queue<ICommand>>("Game.Queue").Enqueue(injectCommand);
    }
}
