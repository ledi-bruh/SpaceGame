namespace SpaceGame.Lib;


public interface IReceiver
{
    ICommand Receive();
    bool isEmpty();
}
