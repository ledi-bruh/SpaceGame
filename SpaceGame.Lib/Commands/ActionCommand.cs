namespace SpaceGame.Lib;


public class ActionCommand : ICommand
{
    private Action _action;
    
    public ActionCommand(Action action)
    {
        _action = action;
    }

    public void Execute() => _action();
}
