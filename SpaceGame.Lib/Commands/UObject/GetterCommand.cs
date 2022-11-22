namespace SpaceGame.Lib;

public class GetterCommand : ICommand
{
    private IUObject _uObject;
    private string _commandName;

    public GetterCommand(IUObject uObject, string commandName)
    {
        _uObject = uObject;
        _commandName = commandName;
    }

    public void Execute() => IoC.Resolve<ICommand>("Game.UObject.GetProperty", _uObject, _commandName).Execute();
}
