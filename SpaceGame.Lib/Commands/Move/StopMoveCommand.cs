namespace SpaceGame.Lib;
class StopMoveCommand: ICommand
{
    private IStopable _stopable;

    StopMoveCommand(IStopable stopable)
    {
        _stopable = stopable;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.Commands.DeleteProperty",_stopable.Target,"Velocity").Execute(); 
        IoC.Resolve<ICommand>(
            "Game.Commands.SetupCommand",
            _stopable.Target,
            "Movement",
            IoC.Resolve<ICommand>("Game.Commands.Empty")
        ).Execute();
    }
}
