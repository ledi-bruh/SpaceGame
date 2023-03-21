namespace SpaceGame.Lib;
using Hwdtech;

public class SetDefaultHandlerCommand : ICommand  // "Exception.Handler.Default.Set"
{
    private IHandler _handler;

    public SetDefaultHandlerCommand(IHandler handler) => _handler = handler;

    public void Execute()
    {
        var tree = IoC.Resolve<IDictionary<int, IHandler>>("Exception.Handler.Tree");

        tree[0] = _handler;
    }
}
