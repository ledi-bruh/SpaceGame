namespace SpaceGame.Lib;


public interface IInterpretetingMessage
{
    public int GameID { get; }
    public int ObjectID { get; }
    public string TypeCommand { get; }
    public IDictionary<string, object> Parameters { get; }
}
