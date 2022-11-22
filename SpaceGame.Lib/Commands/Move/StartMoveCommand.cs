namespace SpaceGame.Lib;

public class StartMoveCommand : ICommand
{
    private IMoveStartable _moveStartable;

    public StartMoveCommand(IMoveStartable moveStartable) => _moveStartable = moveStartable;

    public void Execute()
    {
        IoC.Resolve<ICommand>(
            "Game.UObject.SetProperty",
            _moveStartable.Target,
            "Velocity",
            _moveStartable.Velocity
        ).Execute();

        ICommand command = IoC.Resolve<ICommand>("Game.Operation.Move", _moveStartable.Target);

        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Push(command);
    }
}
