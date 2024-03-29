namespace SpaceGame.Lib;
using Hwdtech;

public class RegisterHandlerCommand : ICommand
{
    private IEnumerable<Type> _types;
    private IHandler _handler;

    public RegisterHandlerCommand(IEnumerable<Type> types, IHandler handler)
    {
        _types = types;
        _handler = handler;
    }

    public void Execute()
    {
        IoC.Resolve<IDictionary<int, IHandler>>("Exception.Handler.Tree").Add(
            IoC.Resolve<int>("GetHashCode.AnyOrder", _types),
            _handler
        );
    }
}
