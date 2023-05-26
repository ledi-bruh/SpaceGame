namespace SpaceGame.Lib;

public class CompileStrategy : IStrategy  // "Compile"
{
    public object Invoke(params object[] args)
    {
        var codeString = (string)args[0];

        return new CompileCommand(codeString);
    }
}
