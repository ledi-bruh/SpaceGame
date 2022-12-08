namespace SpaceGame.Lib;

public interface IUObject
{
    public object GetProperty(string name);
    public void SetProperty(string name, object value);
}
