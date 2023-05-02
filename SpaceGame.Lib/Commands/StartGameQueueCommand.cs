namespace SpaceGame.Lib;
using Hwdtech;


public class StartGameQueueCommand : ICommand
{
    private Queue<ICommand> _queue;

    private int _quantum;

    public StartGameQueueCommand(Queue<ICommand> queue, int quantum)
    {
        _queue = queue;
        _quantum = quantum;
    }


    public void Execute()
    {
        var startTime = DateTime.Now;

        var endTime = DateTime.Now;

        while ((endTime.Subtract(startTime)).TotalMilliseconds <= _quantum)
        {
            var cmd = IoC.Resolve<ICommand>("Game.Queue.Dequeue", _queue);
            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                e.Data["CmdType"] = cmd.GetType();
                IoC.Resolve<IHandler>("Exception.Handler.Find", e).Handle();
            }
            endTime = DateTime.Now;
        }
    }
}
