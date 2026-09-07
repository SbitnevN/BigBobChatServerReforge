using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Template;

namespace BigBob.Generators.DI.Template;

internal class ConstructorTemplateBuilder
{
    private readonly CompilationUnitSyntax _unit;
    private readonly ClassDeclarationSyntax _originalClass;
    private readonly ConstructorDeclarationSyntax _originalConstructor;
    private readonly ExpressionStatementSyntax _resolveTemplate;

    private string _className;
    private ConstructorDeclarationSyntax _constructor;

    public ConstructorTemplateBuilder(CompilationUnitSyntax unit, string @namespace)
    {
        BaseNamespaceDeclarationSyntax oldNamespace = unit.DescendantNodes()
            .OfType<BaseNamespaceDeclarationSyntax>()
            .First();

        BaseNamespaceDeclarationSyntax newNamespace = oldNamespace.WithName(SyntaxFactory.ParseName(@namespace));

        _unit = unit.ReplaceNode(oldNamespace, newNamespace);

        _originalClass = _unit.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .First(i => i.Identifier.Text == nameof(ConstructorTemplate));

        _originalConstructor = _originalClass.DescendantNodes()
            .OfType<ConstructorDeclarationSyntax>()
            .First(i => i.Identifier.Text == nameof(ConstructorTemplate));

        _resolveTemplate = (ExpressionStatementSyntax)_originalConstructor.Body!.Statements.First();
        _className = _originalClass.Identifier.Text;
        _constructor = _originalConstructor.WithBody(SyntaxFactory.Block());
    }

    public void ReplaceClassName(string newName)
    {
        _className = newName;
        _constructor = _constructor.WithIdentifier(SyntaxFactory.Identifier(newName));
    }

    public void AddStartup(string name)
    {
        ExpressionStatementSyntax call = SyntaxFactory.ExpressionStatement(
            SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(name)));

        _constructor = _constructor.WithBody(_constructor.Body!.AddStatements(call));
    }

    public void ResolveServices(IEnumerable<PropertyModel> properties)
    {
        foreach (PropertyModel property in properties)
            ResolveService(property);
    }

    public void ResolveService(PropertyModel property)
    {
        InvocationExpressionSyntax invocation =
            (InvocationExpressionSyntax)_resolveTemplate.Expression;

        MemberAccessExpressionSyntax access =
            (MemberAccessExpressionSyntax)invocation.Expression;

        GenericNameSyntax method =
            (GenericNameSyntax)access.Name;

        method = method.WithTypeArgumentList(
            SyntaxFactory.TypeArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.ParseTypeName(property.Type))));

        invocation = invocation.ReplaceNode(access.Name, method);

        StatementSyntax assignment =
            SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(property.Name),
                    invocation));

        _constructor = _constructor.AddBodyStatements(assignment);
    }

    public CompilationUnitSyntax Build()
    {
        ClassDeclarationSyntax updatedClass = _originalClass.ReplaceNode(_originalConstructor, _constructor);
        updatedClass = updatedClass.WithIdentifier(SyntaxFactory.Identifier(_className));
        return _unit.ReplaceNode(_originalClass, updatedClass);
    }
}
