using BigBob.Generators.Extensions.Builder;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigBob.Generators;

public abstract class UnitBuilderBase
{
    public PredefinedTypeSyntax Void => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));

    public abstract CompilationUnitSyntax Build();

    public InvocationExpressionSyntax CreateCall(string name)
    {
        return SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(name));
    }

    public InvocationExpressionSyntax CreateCall(ExpressionSyntax parent, SimpleNameSyntax method)
    {
        return SyntaxFactory.InvocationExpression(
            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, parent, method));
    }

    public AssignmentExpressionSyntax CreateAssignment(ExpressionSyntax target, ExpressionSyntax expression)
    {
        return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, target, expression);
    }

    public ParameterSyntax CreateParameter(TypedName typed)
    {
        return SyntaxFactory.Parameter(typed.Name.ToIdentifier()).WithType(typed.Type.ToName());
    }

    public ParameterListSyntax CreateParameters(params TypedName[] typedNames)
    {
        return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(typedNames.Select(CreateParameter)));
    }

    public ArgumentSyntax CreateArgument(ExpressionSyntax arg)
    {
        return SyntaxFactory.Argument(arg);
    }

    public ArgumentListSyntax CreateArguments(params string[] names)
    {
        return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(names.Select(name => CreateArgument(name.ToName()))));
    }

    public ArgumentListSyntax CreateArguments(params ExpressionSyntax[] args)
    {
        return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(args.Select(CreateArgument)));
    }

    public SyntaxTokenList CreateModifiers(params SyntaxKind[] kinds)
    {
        return CreateTokens(kinds.Select(SyntaxFactory.Token).ToArray());
    }

    public SyntaxTokenList CreateTokens(params SyntaxToken[] tokens)
    {
        return SyntaxFactory.TokenList(tokens);
    }

    public GenericNameSyntax CreateGenericName(string name, params string[] types)
    {
        return SyntaxFactory.GenericName(name.ToIdentifier(),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList<TypeSyntax>(types.Select(SyntaxFactory.IdentifierName))));
    }

    public override string ToString()
    {
        CompilationUnitSyntax unit = Build();
        return unit.NormalizeWhitespace().ToFullString();
    }
}
