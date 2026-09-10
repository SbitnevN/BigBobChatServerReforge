using BigBob.Generators.Extensions.Builder;
using BigBob.Generators.Extensions.Generator;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigBob.Generators.DI.Registration;

internal class RegistrationBuilder : UnitBuilderBase
{
    private NamespaceDeclarationSyntax _namespace;

    private List<ExpressionStatementSyntax> _serviceRegisters = new();

    public RegistrationBuilder(string @namespace)
    {
        _namespace = SyntaxFactory.NamespaceDeclaration(@namespace.ToName());
    }

    public void Register(RegistrationModel model)
    {
        switch (model.LifeTime, model.IsOpenGeneric)
        {
            case (LifeTime.Singleton, false):
                RegisterSingleton(model);
                break;

            case (LifeTime.Singleton, true):
                RegisterOpenGenericSingleton(model);
                break;

            case (LifeTime.Scoped, false):
                RegisterScoped(model);
                break;

            case (LifeTime.Scoped, true):
                RegisterOpenGenericScoped(model);
                break;

            case (LifeTime.Transient, false):
                RegisterTransient(model);
                break;

            case (LifeTime.Transient, true):
                RegisterOpenGenericTransient(model);
                break;
        }
    }

    public void RegisterOpenGenericSingleton(RegistrationModel @class)
    {
        _serviceRegisters.Add(GetOpenGenericRegister(@class, Methods.AddSingleton).ToStatement());
    }

    public void RegisterSingleton(RegistrationModel @class)
    {
        _serviceRegisters.Add(GetGenericRegister(@class, Methods.AddSingleton).ToStatement());
    }

    public void RegisterOpenGenericScoped(RegistrationModel @class)
    {
        _serviceRegisters.Add(GetOpenGenericRegister(@class, Methods.AddScoped).ToStatement());
    }

    public void RegisterScoped(RegistrationModel @class)
    {
        _serviceRegisters.Add(GetGenericRegister(@class, Methods.AddScoped).ToStatement());
    }

    public void RegisterOpenGenericTransient(RegistrationModel @class)
    {
        _serviceRegisters.Add(GetOpenGenericRegister(@class, Methods.AddTransient).ToStatement());
    }

    public void RegisterTransient(RegistrationModel @class)
    {
        _serviceRegisters.Add(GetGenericRegister(@class, Methods.AddTransient).ToStatement());
    }

    public override CompilationUnitSyntax Build()
    {
        MethodDeclarationSyntax extension = CreateExtension();
        ClassDeclarationSyntax @class = CreateClass(extension);

        NamespaceDeclarationSyntax @namespace =
            _namespace.WithMembers([@class]);

        return SyntaxFactory.CompilationUnit()
            .WithMembers([@namespace]);
    }

    private ExpressionSyntax GetGenericRegister(RegistrationModel @class, string method)
    {
        ExpressionSyntax creation = SyntaxFactory.ObjectCreationExpression(@class.Type.ToName())
            .WithArgumentList(CreateArguments(Variables.Provider.ToName()));

        ExpressionSyntax factory = SyntaxFactory.ParenthesizedLambdaExpression(
            CreateParameters(Variables.Provider), creation);

        InvocationExpressionSyntax call = CreateCall(Variables.Services.ToName(), CreateGenericName(method, @class.Interface, @class.Type));
        if (@class.NeedFactory)
            return call.WithArgumentList(CreateArguments(factory));

        return call;
    }

    private ExpressionSyntax GetOpenGenericRegister(RegistrationModel @class, string method)
    {
        ExpressionSyntax type = CreateCall(Methods.Typeof)
            .WithArgumentList(CreateArguments(@class.Type));

        ExpressionSyntax interfaceType = CreateCall(Methods.Typeof)
            .WithArgumentList(CreateArguments(@class.Interface));

        return CreateCall("services".ToName(), "AddSingleton".ToName())
            .WithArgumentList(CreateArguments(interfaceType, type));
    }

    private MethodDeclarationSyntax CreateExtension()
    {
        return SyntaxFactory.MethodDeclaration(Void, Methods.AddBigBob)
            .WithBody(SyntaxFactory.Block(_serviceRegisters))
            .WithParameterList(CreateExtensionParameters())
            .WithModifiers(CreateModifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword));
    }

    private ClassDeclarationSyntax CreateClass(MethodDeclarationSyntax extension)
    {
        return SyntaxFactory.ClassDeclaration("ServiceCollectionExtensions")
            .WithMembers([extension])
            .WithModifiers(CreateModifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword));
    }

    private ParameterListSyntax CreateExtensionParameters()
    {
        ParameterSyntax parameter = CreateParameter(Variables.Services)
            .WithModifiers(CreateModifiers(SyntaxKind.ThisKeyword));

        return SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(parameter));
    }
}
