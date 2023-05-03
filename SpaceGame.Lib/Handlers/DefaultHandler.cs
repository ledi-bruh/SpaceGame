namespace SpaceGame.Lib;

public class DefaultHandler : IHandler  
{
    private Exception _exception;

    public DefaultHandler(Exception e) => _exception = e;

    public void Handle()
    {
        throw _exception;
    }
}
