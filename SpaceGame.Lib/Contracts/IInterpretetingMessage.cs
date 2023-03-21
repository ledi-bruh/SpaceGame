namespace SpaceGame.Lib;


public interface IInterpretetingMessage
{
    public int game_id { get; }
    public int object_id { get;}
    public string typeCommand {get;}
    public IDictionary<string, object> args {get; }     
}
