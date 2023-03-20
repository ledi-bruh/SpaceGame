namespace SpaceGame.Lib;


public interface ISender
{
    public void Send(ICommand cmd);
}
