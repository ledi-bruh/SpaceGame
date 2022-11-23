namespace SpaceGame.Lib;

public class SetPropertyCommand : ICommand
{
    private IUObject _uObject;
    private string _name;
    private object _value;

    public SetPropertyCommand(IUObject uObject, string name, object value)
    {
        _uObject = uObject;
        _name = name;
        _value = value;
    }

    public void Execute() => _uObject.SetProperty(_name, _value);
}
