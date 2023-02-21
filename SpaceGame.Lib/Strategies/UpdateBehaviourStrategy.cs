namespace SpaceGame.Lib;


public class UpdateBehaviourStrategy : IStrategy
{
    public object Invoke(params object[] args)
    {
        ServerThread thread = (ServerThread)args[0];
        Action behaviour = (Action)args[1];
        return new UpdateBehaviourCommand(thread, behaviour);
    }
}
