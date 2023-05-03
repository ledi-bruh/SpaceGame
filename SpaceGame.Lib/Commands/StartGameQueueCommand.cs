namespace SpaceGame.Lib;
using Hwdtech;
using System.Diagnostics;


public class StartGameQueueCommand : ICommand
{
    private Queue<ICommand> _queue;


    public StartGameQueueCommand(Queue<ICommand> queue)
    {
        _queue = queue;
    }


    public void Execute()
    {
        var stopwatch = new Stopwatch();

        stopwatch.Start();

        while (stopwatch.ElapsedMilliseconds <= IoC.Resolve<int>("Game.Get.Time.Quantum"))
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
        }
        stopwatch.Stop();
    }
}
