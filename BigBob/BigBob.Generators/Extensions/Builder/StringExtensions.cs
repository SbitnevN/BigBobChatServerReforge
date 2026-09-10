using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigBob.Generators.Extensions.Builder;

internal static class StringExtensions
{
    public static bool IsEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }

    public static bool Any(this string text)
    {
        return !text.IsEmpty();
    }

    public static IdentifierNameSyntax ToName(this string name)
    {
        return SyntaxFactory.IdentifierName(name);
    }

    public static SyntaxToken ToIdentifier(this string name)
    {
        return SyntaxFactory.Identifier(name);
    }
}
