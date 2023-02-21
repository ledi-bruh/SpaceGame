namespace SpaceGame.Lib;

public class ServerThread
{
    Thread thread;

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

        thread = new Thread(() =>
        {
            while (!stop)
                strategy();
            
        });
    }

    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }

    public void Execute()
    {
        thread.Start();
    }
}
