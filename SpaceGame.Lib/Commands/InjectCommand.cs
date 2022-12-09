namespace SpaceGame.Lib;

public class InjectCommand : ICommand, IInjectable
{
    private ICommand _command;

    public InjectCommand(ICommand command) => _command = command;

    public void Execute() => _command.Execute();

    public void Inject(object command) => _command = (ICommand)command;
}
