namespace SpaceGame.Lib;

public class InjectCommand : ICommand, IInjectable
{
    ICommand _cmd;

    public InjectCommand(ICommand cmd) => _cmd = cmd;

    public void Execute() => _cmd.Execute();

    public void Inject(object obj) => _cmd = (ICommand)obj; 
}
