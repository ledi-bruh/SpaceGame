namespace SpaceGame.Lib;

public class ServerThread
{
    private Thread _thread;
    IReceiver queue;
    bool stop = false;
    Action strategy;

    internal void Stop() => stop = true;

    internal void HandleCommand()
    {
        var cmd = queue.Receive();

        cmd.Execute();
    }

    public ServerThread(IReceiver queue)
    {
        this.queue = queue;
        strategy = () =>
        {
            HandleCommand();
        };

        _thread = new Thread(() =>
        {
            while (!stop)
                strategy();
        });
    }

    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }
    public void Start()
    {
        _thread.Start();
    }

    internal bool isEmpty()
    {
        return queue.isEmpty();
    }

    internal bool isCurrent()
    {
        return _thread == Thread.CurrentThread;
    }
}
