using BigBob.Generators.Extensions.Builder;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigBob.Generators.Extensions.Generator;

internal static class TypedNameExtensions
{
    public static IdentifierNameSyntax ToName(this TypedName typedName)
    {
        return typedName.Name.ToName();
    }

    public static SyntaxToken Identifier(this TypedName typedName)
    {
        return typedName.Name.ToIdentifier();
    }
}
