namespace SpaceGame.Lib;

public class CreateLogFileHandlerStrategy : IStrategy  // "Exception.Handler.Log.Create"
{
    public object Invoke(params object[] args)
    {
        var log_file_path = (string)args[0];
        var types = (IEnumerable<Type>)args[1];

        return new LogFileHandler(log_file_path, types);
    }
}
