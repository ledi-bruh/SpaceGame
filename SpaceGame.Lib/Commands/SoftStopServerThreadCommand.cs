namespace SpaceGame.Lib;
using Hwdtech;


public class SoftStopServerThreadCommand : ICommand
{
    ServerThread _thread;
    Action _action;

    public SoftStopServerThreadCommand(ServerThread thread, Action action)
    {
        _thread = thread;
        _action = action;
    }
    public void Execute()
    {
        if (_thread.isCurrent())
        {
            _thread.UpdateBehaviour(() =>
            {
                if (_thread.isEmpty())
                {
                    _thread.Stop();
                    _action();
                }
                else
                {
                    _thread.HandleCommand();
                }
            });
        }
        else
        {
            throw new Exception();
        }
    }
}
