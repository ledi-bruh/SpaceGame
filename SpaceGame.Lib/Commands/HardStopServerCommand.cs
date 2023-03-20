namespace SpaceGame.Lib;
using Hwdtech;


public class HardStopServerCommand : ICommand
{
    ServerThread _thread;

    public HardStopServerCommand(ServerThread thread) => _thread = thread;

    public void Execute()
    {
        if(_thread.isCurrent())
        {
            _thread.Stop();   
        }
        else
        {
            throw new Exception();
        }
    }
}
