namespace SpaceGame.Lib;
using Hwdtech;

public class InterpretCommand : ICommand 
{
    IInterpretingMessage _message;

    public InterpretCommand(IInterpretingMessage message) => _message = message;

    public void Execute()
    {
        var cmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.Create.FromMessage", _message);

        IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Queue.Push", _message.GameID, cmd).Execute();
    }
}
