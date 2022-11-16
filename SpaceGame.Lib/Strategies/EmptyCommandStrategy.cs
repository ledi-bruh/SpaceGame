namespace SpaceGame.Lib;

class GetEmptyCommandStrategy : IStrategy
{
    ICommand _command; // без сокращений

    public GetEmptyCommandStrategy()
    {
        _command = new EmptyCommand();
    }
    public object Invoke(params object[] args)
    {
        return _command;
    }
}