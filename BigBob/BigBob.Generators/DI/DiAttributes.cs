using BigBob.Generators.Extensions.Generator;
using Microsoft.CodeAnalysis;

namespace BigBob.Generators.DI;

internal class DiAttributes
{
    public const string Transient = "TransientAttribute";
    public const string Singleton = "SingletonAttribute";
    public const string Scoped = "ScopedAttribute";
}

internal class DiGenericAttributes
{
    public const string Transient = "TransientAttribute`1";
    public const string Singleton = "SingletonAttribute`1";
    public const string Scoped = "ScopedAttribute`1";
}

internal static class DiAttributeHelper
{
    public static bool IsSingleton(string metadata)
    {
        return metadata is DiAttributes.Singleton or DiGenericAttributes.Singleton;
    }

    public static bool IsTransient(string metadata)
    {
        return metadata is DiAttributes.Transient or DiGenericAttributes.Transient;
    }

    public static bool IsScoped(string metadata)
    {
        return metadata is DiAttributes.Scoped or DiGenericAttributes.Scoped;
    }

    public static AttributeData? GetSingleton(INamedTypeSymbol @class)
    {
        return @class.GetAttribute(DiGenericAttributes.Singleton) ?? @class.GetAttribute(DiAttributes.Singleton);
    }

    public static AttributeData? GetTransient(INamedTypeSymbol @class)
    {
        return @class.GetAttribute(DiGenericAttributes.Transient) ?? @class.GetAttribute(DiAttributes.Transient);
    }

    public static AttributeData? GetScoped(INamedTypeSymbol @class)
    {
        return @class.GetAttribute(DiGenericAttributes.Scoped) ?? @class.GetAttribute(DiAttributes.Scoped);
    }
}
