namespace SpaceGame.Lib;
using Hwdtech;


public class HardStopServerThreadCommand : ICommand
{
    ServerThread _thread;

    public HardStopServerThreadCommand(ServerThread thread) => _thread = thread;

    public void Execute()
    {
        if(_thread.isCurrent())
        {
            _thread.Stop();   
        }
        else throw new Exception();
    }
}
