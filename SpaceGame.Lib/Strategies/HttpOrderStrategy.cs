namespace SpaceGame.Lib;
using Hwdtech;

public class HttpOrderStrategy : IStrategy  // Http.Order
{
    public object Invoke(params object[] args)
    {
        var message = (IInterpretingMessage)args[0];

        var map = IoC.Resolve<IDictionary<int, int>>("Server.Game.Thread.Map");

        var command = IoC.Resolve<ICommand>("Game.Command.Interpret", message);

        return IoC.Resolve<ICommand>("Server.Thread.Command.Send", map[message.GameID], command);
    }
}
