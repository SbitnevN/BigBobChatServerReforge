using BigBob.Generators.DI.Extensions;
using BigBob.Generators.Extensions.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace BigBob.Generators.DI.Registration;

[Generator]
public class RegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<RegistrationModel?> registrations = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
            static (context, _) => CreateModel(context.GetSymbol<INamedTypeSymbol>()));

        context.RegisterSourceOutput(registrations.Where(static model => model is not null).Collect(), Generate);
    }

    private static void Generate(SourceProductionContext context, ImmutableArray<RegistrationModel?> models)
    {
        RegistrationBuilder builder = new("BigBob.API");

        foreach (RegistrationModel? model in models)
        {
            if (model is not null)
                builder.Register(model);
        }

        context.AddSource("ServicesCollectionExtensions.g.cs", builder.ToString());
    }

    internal static RegistrationModel? CreateModel(INamedTypeSymbol? type)
    {
        if (type is null || !type.TryGetRegistration(out AttributeData? attribute, out LifeTime lifeTime))
            return null;

        INamedTypeSymbol interfaceType = attribute!.GetRegistrationType()
            ?? throw new InvalidOperationException($"Cannot determine interface for {type.Name}");

        bool isOpenGeneric = interfaceType.IsUnboundGenericType;

        return new RegistrationModel
        {
            Type = type.ToRegistrationString(isOpenGeneric),
            Interface = interfaceType.ToRegistrationString(isOpenGeneric),
            IsOpenGeneric = isOpenGeneric,
            LifeTime = lifeTime,
            NeedFactory = type.HasServiceMembers()
        };
    }
}