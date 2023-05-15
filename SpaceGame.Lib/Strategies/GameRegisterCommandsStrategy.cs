namespace SpaceGame.Lib;


public class GameRegisterCommandsStrategy : IStrategy //Game.Register.Commands
{
    public object Invoke(params object[] args)
    {
        return new GameRegisterCommandsCommand();
    }
}
