namespace SpaceGame.Lib;
using Hwdtech;

public class RegisterHandlerCommand : ICommand
{
    private object _command, _exception;
    private IHandler _handler;

    public RegisterHandlerCommand(object command, object exception, IHandler handler)
    {
        _command = command;
        _exception = exception;
        _handler = handler;
    }

    public void Execute()
    {
        var tree = IoC.Resolve<IDictionary<object, IDictionary<object, IHandler>>>("Exception.Handle.Tree");
        tree.TryAdd(_command, new Dictionary<object, IHandler>());
        tree[_command].Add(_exception, _handler);
    }
}
