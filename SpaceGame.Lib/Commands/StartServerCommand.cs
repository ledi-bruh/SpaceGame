namespace SpaceGame.Lib;
using Hwdtech;

public class StartServerCommand : ICommand
{
    private int _threadCount;

    public StartServerCommand(int threadCount) => _threadCount = threadCount;

    public void Execute()
    {
        for (int i = 0; i < _threadCount; i++)
        {
            IoC.Resolve<ICommand>("Server.Thread.Create.Start", i).Execute();
        }
    }
}
