namespace SpaceGame.Lib;

public class LogFileHandler : IHandler  // "Exception.Handler.Log.Create"
{
    private string _log_file_path;
    private IEnumerable<Type> _types;

    public LogFileHandler(string log_file_path, IEnumerable<Type> types)
    {
        _log_file_path = log_file_path;
        _types = types;
    }

    public void Handle()
    {
        string line = String.Join(" ~ ", _types.Select(t => t.ToString())) + Environment.NewLine;
        File.AppendAllText(_log_file_path, line);
    }
}
