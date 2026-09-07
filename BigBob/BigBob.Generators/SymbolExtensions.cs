using Microsoft.CodeAnalysis;

namespace BigBob.Generators;

internal static partial class SymbolExtensions
{
    public static IEnumerable<T> WithAttribute<T>(this IEnumerable<T> symbols, string attributeFullName) where T : ISymbol
    {
        return symbols.Where(i => i.HasAttribute(attributeFullName));
    }

    public static bool HasAttribute<T>(this T symbol, string attributeFullName) where T : ISymbol
    {
        return symbol.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == attributeFullName);
    }
}
