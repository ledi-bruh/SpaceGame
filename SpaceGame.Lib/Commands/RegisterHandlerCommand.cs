namespace SpaceGame.Lib;
using Hwdtech;

public class RegisterHandlerCommand : ICommand
{
    private ICommand _command;
    private Exception _exception;
    private IHandler _handler;

    public RegisterHandlerCommand(ICommand command, Exception exception, IHandler handler)
    {
        _command = command;
        _exception = exception;
        _handler = handler;
    }

    public void Execute()
    {
        IoC.Resolve<IDictionary<ICommand, IDictionary<Exception, IHandler>>>("Game.HandlerTree")[_command].Add(_exception, _handler);
    }
}
