namespace SpaceGame.Lib;
using System.Collections.Concurrent;
using Hwdtech;

public class RepeatConcurrentCommand : ICommand  // "Command.Concurrent.Repeat"
{
    private ICommand _command;

    public RepeatConcurrentCommand(ICommand command) => _command = command;

    public void Execute() => IoC.Resolve<BlockingCollection<ICommand>>("Server.Thread.Queue").Add(_command);
}
