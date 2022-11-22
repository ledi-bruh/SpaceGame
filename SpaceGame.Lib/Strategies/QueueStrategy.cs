namespace SpaceGame.Lib;

public class QueueStrategy : IStrategy  // "Game.Queue"
{
    private IQueue<ICommand> _queue;

    public QueueStrategy(IQueue<ICommand> queue) => _queue = queue;

    public object Invoke(params object[] args) => _queue;
}
