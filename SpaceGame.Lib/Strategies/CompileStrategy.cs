namespace SpaceGame.Lib;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Hwdtech;

public class CompileStrategy : IStrategy  // "Compile"
{
    public object Invoke(params object[] args)
    {
        var codeString = (string)args[0];

        var assemblyName = IoC.Resolve<string>("Assembly.Name.Create");
        var references = IoC.Resolve<IEnumerable<MetadataReference>>("Compile.References");
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        var syntaxTree = CSharpSyntaxTree.ParseText(codeString);

        var compilation = CSharpCompilation.Create(assemblyName).AddReferences(references);
        compilation = compilation.WithOptions(options).AddSyntaxTrees(syntaxTree);

        Assembly assembly;

        using (var ms = new System.IO.MemoryStream())
        {
            var result = compilation.Emit(ms);
            ms.Seek(0, SeekOrigin.Begin);
            assembly = Assembly.Load(ms.ToArray());
        }

        return assembly;
    }
}
