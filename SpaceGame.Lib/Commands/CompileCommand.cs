namespace SpaceGame.Lib;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Hwdtech;

public class CompileCommand : ICommand
{
    private string _codeString;

    public CompileCommand(string codeString) => _codeString = codeString;

    public void Execute()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(_codeString);
        var assemblyName = Guid.NewGuid().ToString();
        var references = IoC.Resolve<IEnumerable<MetadataReference>>("Compile.References");

        var compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var ms = new System.IO.MemoryStream();
        compilation.Emit(ms);
    }
}
