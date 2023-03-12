namespace SpaceGame.Lib;
using Hwdtech;

public class SetDefaultHandlerCommand : ICommand
{
    private IHandler _handler;

    public SetDefaultHandlerCommand(IHandler handler) => _handler = handler;

    public void Execute()
    {
        var tree = IoC.Resolve<IDictionary<int, IHandler>>("Exception.Handler.Tree");

        tree[0] = _handler;
    }
}
