using BigBob.Generators.Extensions.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigBob.Generators.DI.Constructor;

[Generator]
public class ConstructorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<INamedTypeSymbol> classes = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax,
                static (context, _) => context.GetSymbol<INamedTypeSymbol>())
            .Where(i => i != null)!;

        context.RegisterSourceOutput(classes, ResolveClasses);
    }

    private void ResolveClasses(SourceProductionContext context, INamedTypeSymbol @class)
    {
        ConstructorBuilder builder = new(@class.GetNamespace(), @class.Name);

        IEnumerable<IPropertySymbol> properties = @class.GetMembers().OfType<IPropertySymbol>();
        bool hasService = ResolveServices(properties, builder);

        IEnumerable<IMethodSymbol> methods = @class.GetMembers().OfType<IMethodSymbol>();
        bool hasStartup = ResolveStartups(methods, builder);

        if (hasStartup || hasService)
            context.AddSource($"{@class.Name}.g.cs", builder.ToString());
    }

    private bool ResolveServices(IEnumerable<IPropertySymbol> properties, ConstructorBuilder builder)
    {
        properties = properties.WithAttribute("ServiceAttribute");
        builder.ResolveServices(properties.Select(PropertyModel.FromSymbol));

        return properties.Any();
    }

    private bool ResolveStartups(IEnumerable<IMethodSymbol> methods, ConstructorBuilder builder)
    {
        methods = methods.WithAttribute("StartupAttribute");
        foreach (IMethodSymbol method in methods)
            builder.AddStartup(method.Name);

        return methods.Any();
    }
}
