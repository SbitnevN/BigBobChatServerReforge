using Microsoft.CodeAnalysis;

namespace BigBob.Generators.DI;

public class PropertyModel
{
    public INamedTypeSymbol Parent { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string Type { get; private set; } = null!;

    public static PropertyModel FromSymbol(IPropertySymbol property)
    {
        return new PropertyModel
        {
            Parent = property.ContainingType,
            Name = property.Name,
            Type = property.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
        };
    }
}
