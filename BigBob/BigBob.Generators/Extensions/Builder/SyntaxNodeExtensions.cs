using Microsoft.CodeAnalysis;

namespace BigBob.Generators.Extensions.Builder;

internal static class SyntaxNodeExtensions
{
    public static T? GetNode<T>(this SyntaxNode parent) where T : SyntaxNode
    {
        return parent.DescendantNodes().OfType<T>().FirstOrDefault();
    }

    public static T? GetNode<T>(this SyntaxNode parent, Func<T, bool> predicate) where T : SyntaxNode
    {
        return parent.DescendantNodes().OfType<T>().FirstOrDefault(predicate);
    }

    public static T GetRequiredNode<T>(this SyntaxNode parent) where T : SyntaxNode
    {
        return parent.DescendantNodes().OfType<T>().First();
    }

    public static T GetRequiredNode<T>(this SyntaxNode parent, Func<T, bool> predicate) where T : SyntaxNode
    {
        return parent.DescendantNodes().OfType<T>().First(predicate);
    }
}
