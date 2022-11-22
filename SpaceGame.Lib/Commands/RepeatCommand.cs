namespace SpaceGame.Lib;

public class RepeatCommand : ICommand
{
    private ICommand _command;

    public RepeatCommand(ICommand command) => _command = command;

    public void Execute() => IoC.Resolve<IQueue<ICommand>>("Game.Queue").Push(_command);
}
