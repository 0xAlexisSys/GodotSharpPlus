using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GodotSharpPlus.SourceGen;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SingletonNodeAttributeAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor _gdp0001Rule = new(
        "GDP0001",
        $"{SingletonNodeAttributeInfo.Name} is not applicable to classes that do not inherit from {NodeName}",
        $$"""Class '{0}' does not inherit from {{NodeName}} and cannot be decorated with {{SingletonNodeAttributeInfo.Name}}""",
        "Usage",
        DiagnosticSeverity.Error,
        true,
        customTags: WellKnownDiagnosticTags.NotConfigurable
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
    [
        _gdp0001Rule,
    ];

    public override void Initialize(AnalysisContext initContext)
    {
        initContext.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        initContext.EnableConcurrentExecution();

        initContext.RegisterSyntaxNodeAction(static context =>
        {
            if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax || context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not {IsNode: false} classSymbol || classSymbol.GetAttributeData(SingletonNodeAttributeInfo.QualifiedName) is null)
            {
                return;
            }

            Diagnostic diagnostic = Diagnostic.Create(_gdp0001Rule, classDeclarationSyntax.Identifier.GetLocation(), classDeclarationSyntax.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }, SyntaxKind.ClassDeclaration);
    }
}
