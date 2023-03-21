namespace SpaceGame.Lib;
using Hwdtech;


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
        try
        {
            cmd.Execute();
        }
        catch (Exception e)
        {
            IoC.Resolve<IHandler>("Exception.Handler.Find", new List<Type>{cmd.GetType(), e.GetType()}).Handle();
        }
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
