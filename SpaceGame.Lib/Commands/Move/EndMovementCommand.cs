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

        IoC.Resolve<ICommand>(
            "Game.UObject.SetProperty",
            _endable.Target,
            "Movement",
            IoC.Resolve<ICommand>("Game.Command.Empty")
        ).Execute();
    }
}
