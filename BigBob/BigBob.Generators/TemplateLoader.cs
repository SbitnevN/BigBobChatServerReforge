using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace BigBob.Generators;

internal static class TemplateLoader
{
    public static CompilationUnitSyntax Load<T>() where T : class
    {
        Type type = typeof(T);
        Assembly assembly = type.Assembly;

        string resourceSuffix = $"{type.Name}.template.cs";

        string resourceName = assembly
            .GetManifestResourceNames()
            .Single(x => x.EndsWith(resourceSuffix, StringComparison.Ordinal));

        using Stream stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Template '{resourceName}' not found.");

        using StreamReader reader = new StreamReader(stream);

        string source = reader.ReadToEnd();

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        return syntaxTree.GetCompilationUnitRoot();
    }
}
