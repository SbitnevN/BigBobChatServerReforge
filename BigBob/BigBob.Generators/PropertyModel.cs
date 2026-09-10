using Microsoft.CodeAnalysis;

namespace BigBob.Generators;

public class PropertyModel
{
    public string Name { get; private set; } = string.Empty;

    public string Type { get; private set; } = string.Empty;

    public static PropertyModel FromSymbol(IPropertySymbol property)
    {
        return new PropertyModel
        {
            Name = property.Name,
            Type = property.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
        };
    }
}
