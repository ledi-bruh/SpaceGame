namespace SpaceGame.Lib;

public class EndMovementStrategy : IStrategy  // "IoC.Resolve"
{
    public object Invoke(params object[] args)
    {
        // try catch
        IEndable Target = (IEndable)args[0];

        return new EndMovementCommand(Target);
    }
}
