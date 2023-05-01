namespace SpaceGame.Lib;
using Hwdtech;


public class GameCommand : ICommand
{
    private Queue<ICommand> _queue;

    private object _scope;

    double _quantum;

    public GameCommand(object scope, Queue<ICommand> queue, double quantum)
    {
        _scope = scope;
        _queue = queue;
    }


    public void Execute()
    {
        IoC.Resolve<ICommand>("Scopes.Current.Set", _scope).Execute();

        IoC.Resolve<ICommand>("Game.Command.Queue.Start", _queue, IoC.Resolve<double>("Game.Get.Time.Quantum")).Execute();
    }
}
