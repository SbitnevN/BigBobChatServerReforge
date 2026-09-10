using BigBob.Generators.Extensions.Builder;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigBob.Generators.DI.Constructor;

internal class ConstructorBuilder : UnitBuilderBase
{
    private readonly NamespaceDeclarationSyntax _namespace;

    private readonly List<ExpressionStatementSyntax> _serviceResolves = new();

    private readonly List<ExpressionStatementSyntax> _startups = new();

    private readonly string _className = string.Empty;

    public ConstructorBuilder(string @namespace, string className)
    {
        _namespace = SyntaxFactory.NamespaceDeclaration(@namespace.ToName());
        _className = className;
    }

    public void AddStartup(string name)
    {
        _startups.Add(CreateCall(name).ToStatement());
    }

    public void ResolveServices(IEnumerable<PropertyModel> properties)
    {
        foreach (PropertyModel property in properties)
            ResolveService(property);
    }

    public void ResolveService(PropertyModel property)
    {
        ExpressionSyntax call = CreateCall("provider".ToName(),
            CreateGenericName("GetRequiredService", property.Type));

        ExpressionSyntax assignment = CreateAssignment(property.Name.ToName(), call);

        _serviceResolves.Add(assignment.ToStatement());
    }

    public override CompilationUnitSyntax Build()
    {
        ConstructorDeclarationSyntax constructor = CreateConstructor();
        ClassDeclarationSyntax @class = CreateClass(constructor);

        NamespaceDeclarationSyntax @namespace =
            _namespace.WithMembers([@class]);

        return SyntaxFactory.CompilationUnit()
            .WithMembers([@namespace]);
    }

    private ConstructorDeclarationSyntax CreateConstructor()
    {
        ConstructorDeclarationSyntax constructor = SyntaxFactory.ConstructorDeclaration(_className.ToIdentifier())
            .WithBody(SyntaxFactory.Block(_serviceResolves).AddStatements(_startups.ToArray()))
            .WithModifiers(CreateModifiers(SyntaxKind.PublicKeyword));

        if (_serviceResolves.Any())
            constructor = constructor.WithParameterList(CreateParameters(new TypedName("IServiceProvider", "provider")));

        return constructor;
    }

    private ClassDeclarationSyntax CreateClass(ConstructorDeclarationSyntax constructor)
    {
        return SyntaxFactory.ClassDeclaration(_className.ToIdentifier())
            .WithMembers([constructor])
            .WithModifiers(CreateModifiers(SyntaxKind.PublicKeyword, SyntaxKind.PartialKeyword));
    }
}
