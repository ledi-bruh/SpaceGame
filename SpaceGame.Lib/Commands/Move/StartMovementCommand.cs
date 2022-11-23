namespace SpaceGame.Lib;

public class StartMovementCommand : ICommand
{
    private IStartable _startable;

    public StartMovementCommand(IStartable startable) => _startable = startable;

    public void Execute()
    {
        _startable.Parameters.ToList().ForEach(param =>
            IoC.Resolve<ICommand>(
                "Game.UObject.SetProperty",
                _startable.Target,
                param.Key,
                param.Value
            ).Execute()
        );

        ICommand command = IoC.Resolve<ICommand>("Game.Operation.Movement", _startable.Target);

        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Push(command);
    }
}
