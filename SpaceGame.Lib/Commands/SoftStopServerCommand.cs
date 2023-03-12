namespace SpaceGame.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class SoftStopServerCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ConcurrentDictionary<int, object>>("Server.Thread.Map").ToList().ForEach(
            pair => IoC.Resolve<ICommand>("Server.Thread.Stop.Soft", pair.Key).Execute()
        );
    }
}
