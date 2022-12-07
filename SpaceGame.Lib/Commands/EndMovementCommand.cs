namespace SpaceGame.Lib;
public class EndMovementCommand : ICommand
{
    private IEndable _endable;

    public EndMovementCommand(IEndable endable)
    {
        _endable = endable;
    }

    public void Execute()
    {
        _endable.Keys.ToList().ForEach(key =>
            IoC.Resolve<ICommand>(
                "Game.UObject.DeleteProperty",
                _endable.Target,
                key
            ).Execute()
        );

        IoC.Resolve<IInjectable>(
            "Game.UObject.GetProperty",
            _endable.Target,
            "Movement"
        ).Inject(IoC.Resolve<ICommand>("Game.Command.EmptyCommand"));
    }
}
