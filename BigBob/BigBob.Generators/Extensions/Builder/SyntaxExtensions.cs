using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigBob.Generators.Extensions.Builder;

internal static class SyntaxExtensions
{
    public static ExpressionStatementSyntax ToStatement(this ExpressionSyntax expression)
    {
        return SyntaxFactory.ExpressionStatement(expression);
    }
}
