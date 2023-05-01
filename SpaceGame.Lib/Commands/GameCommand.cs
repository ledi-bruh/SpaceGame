namespace SpaceGame.Lib;
using Hwdtech;
using Hwdtech.Ioc;


public class GameCommand : SpaceGame.Lib.ICommand
{
    private Queue<SpaceGame.Lib.ICommand> _queue;

    private object _scope;

    public GameCommand(object scope, Queue<SpaceGame.Lib.ICommand> queue)
    {
        _scope = scope;
        _queue = queue;
    }


    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.Queue.Start", _queue, IoC.Resolve<int>("Game.Get.Time.Quantum")).Execute();
    }
}
