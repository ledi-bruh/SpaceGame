namespace SpaceGame.Lib;

public class UObject : IUObject
{
    private IDictionary<string, object> _properties;

    public UObject() => _properties = new Dictionary<string, object>();

    public object GetProperty(string name) => _properties[name];

    public void SetProperty(string name, object value) => _properties[name] = value;
}
