using BigBob.Generators.DI;
using BigBob.Generators.DI.Registration;
using Microsoft.CodeAnalysis;

namespace BigBob.Generators.Extensions.Generator;

internal static partial class SymbolExtensions
{
    public static IEnumerable<T> WithAttribute<T>(this IEnumerable<T> symbols, string attributeFullName) where T : ISymbol
    {
        return symbols.Where(i => i.HasAttribute(attributeFullName));
    }

    public static bool HasAttribute<T>(this T symbol, string attributeFullName) where T : ISymbol
    {
        return symbol.GetAttributes().Any(a => a.AttributeClass?.MetadataName == attributeFullName);
    }

    public static AttributeData? GetAttribute<T>(this T symbol, string attributeFullName) where T : ISymbol
    {
        return symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.MetadataName == attributeFullName);
    }

    public static T? GetSymbol<T>(this GeneratorSyntaxContext context) where T : ISymbol
    {
        ISymbol? symbol = context.SemanticModel.GetDeclaredSymbol(context.Node);
        return (T?)symbol;
    }

    public static string GetNamespace(this ISymbol symbol)
    {
        return symbol.ContainingNamespace.ToDisplayString();
    }

    public static string ToFullString(this ISymbol symbol)
    {
        return symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    public static string ToOpenGenericString(this INamedTypeSymbol symbol)
    {
        return symbol.ConstructUnboundGenericType()
            .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    public static bool TryGetRegistration(this INamedTypeSymbol type, out AttributeData? attribute, out LifeTime lifeTime)
    {
        if ((attribute = DiAttributeHelper.GetSingleton(type)) is not null)
        {
            lifeTime = LifeTime.Singleton;
            return true;
        }

        if ((attribute = DiAttributeHelper.GetScoped(type)) is not null)
        {
            lifeTime = LifeTime.Scoped;
            return true;
        }

        if ((attribute = DiAttributeHelper.GetTransient(type)) is not null)
        {
            lifeTime = LifeTime.Transient;
            return true;
        }

        lifeTime = default;
        return false;
    }

    public static string ToRegistrationString(this INamedTypeSymbol type, bool isOpenGeneric)
    {
        return isOpenGeneric ? type.ToOpenGenericString() : type.ToFullString();
    }

    public static bool HasServiceMembers(this INamedTypeSymbol type)
    {
        return type.GetMembers().WithAttribute("ServiceAttribute").Any();
    }
}
