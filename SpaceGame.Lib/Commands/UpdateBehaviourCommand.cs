namespace SpaceGame.Lib;


public class UpdateBehaviourCommand : ICommand
{
    ServerThread _thread;

    Action _behaviour;

    public UpdateBehaviourCommand(ServerThread thread, Action behaviour)
    {
        _thread = thread;
        _behaviour = behaviour;
    }
    public void Execute()
    {
        _thread.UpdateBehaviour(_behaviour);
    }
}
