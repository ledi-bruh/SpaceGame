namespace SpaceGame.Lib;
using Hwdtech;

public class RepeatCommand : ICommand
{
    private ICommand _command;

    public RepeatCommand(ICommand command) => _command = command;

    public void Execute() => IoC.Resolve<Queue<ICommand>>("Game.Queue").Enqueue(_command);
}
