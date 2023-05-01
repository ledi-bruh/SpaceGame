namespace SpaceGame.Lib;

public class DefaultHandler : IHandler  
{

    public DefaultHandler(){}

    public void Handle()
    {
        throw new Exception();
    }
}
