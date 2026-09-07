using BigBob.Generators.DI.Template;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Template;

namespace BigBob.Generators.DI;

[Generator]
public class ConstructorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<PropertyModel> properties =
            context.SyntaxProvider.ForAttributeWithMetadataName(
                "BigBob.DI.ServiceAttribute",
                static (node, _) => node is PropertyDeclarationSyntax,
                static (ctx, _) =>
                {
                    IPropertySymbol property = (IPropertySymbol)ctx.TargetSymbol;
                    return PropertyModel.FromSymbol(property);
                });

        IncrementalValueProvider<ImmutableArray<PropertyModel>> collected = properties.Collect();

        context.RegisterSourceOutput(collected,
            static (context, properties) =>
            {
                IEnumerable<IGrouping<INamedTypeSymbol, PropertyModel>> groups =
                     properties.GroupBy(property => property.Parent, NamedTypeSymbolComparer.Instance);

                foreach (IGrouping<INamedTypeSymbol, PropertyModel> group in groups)
                {
                    INamedTypeSymbol type = group.Key;
                    string className = type.Name;

                    ISymbol? startup = type.GetMembers()
                        .WithAttribute("BigBob.Core.StartupAttribute")
                        .FirstOrDefault();

                    ConstructorTemplateBuilder builder = new(TemplateLoader.Load<ConstructorTemplate>(),
                        type.ContainingNamespace.ToDisplayString());

                    builder.ReplaceClassName(className);
                    builder.ResolveServices(group);

                    if (startup != null)
                        builder.AddStartup(startup.Name);

                    string text = builder.Build().NormalizeWhitespace().ToFullString();
                    context.AddSource($"{className}.g.cs", text);
                }
            });
    }
}
