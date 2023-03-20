namespace SpaceGame.Lib;
using Hwdtech;


public class SoftStopServerCommand : ICommand
{
    ServerThread _thread;
    Action _action;

    public SoftStopServerCommand(ServerThread thread, Action action)
    {
        _thread = thread;
        _action = action;
    }
    public void Execute()
    {
        if (_thread == Thread.CurrentThread)
        {
            _thread.UpdateBehaviour(() =>
            {
                if (_thread.isEmpty())
                {
                    _thread.Stop();
                    _action();
                }
                _thread.HandleCommand();
            });
        }
        else
        {
            throw new Exception();
        }
    }
}
