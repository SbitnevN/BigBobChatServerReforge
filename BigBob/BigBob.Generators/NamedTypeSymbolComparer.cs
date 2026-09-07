using Microsoft.CodeAnalysis;

namespace BigBob.Generators;

internal sealed class NamedTypeSymbolComparer : IEqualityComparer<INamedTypeSymbol>
{
    public static readonly NamedTypeSymbolComparer Instance = new();

    public bool Equals(INamedTypeSymbol? x, INamedTypeSymbol? y)
    {
        return SymbolEqualityComparer.Default.Equals(x, y);
    }

    public int GetHashCode(INamedTypeSymbol obj)
    {
        return SymbolEqualityComparer.Default.GetHashCode(obj);
    }
}
