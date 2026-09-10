using Microsoft.CodeAnalysis;

namespace BigBob.Generators.DI.Extensions;

internal static class AttributeDataExtensions
{
    public static INamedTypeSymbol? GetRegistrationType(this AttributeData attribute)
    {
        INamedTypeSymbol? genericType = attribute.AttributeClass?.TypeArguments.FirstOrDefault() as INamedTypeSymbol;

        if (genericType is not null)
            return genericType;

        return attribute.ConstructorArguments.FirstOrDefault().Value as INamedTypeSymbol;
    }
}