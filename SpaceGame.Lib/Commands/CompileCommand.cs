namespace SpaceGame.Lib;
using Hwdtech;

public class CompileCommand : ICommand
{
    string _codeString;

    public CompileCommand(string codeString) => _codeString = codeString;

    public void Execute()
    {
        ;
    }
}
