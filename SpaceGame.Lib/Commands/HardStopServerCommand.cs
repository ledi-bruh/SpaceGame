namespace SpaceGame.Lib;
using Hwdtech;


public class HardStopServerCommand : ICommand
{
    ServerThread _thread;

    public HardStopServerCommand(ServerThread thread) => _thread = thread;

    public void Execute()
    {
        if(_thread == Thread.CurrentThread)
        {
            _thread.Stop();   
        }
        else
        {
            throw new Exception();
        }
    }
}
