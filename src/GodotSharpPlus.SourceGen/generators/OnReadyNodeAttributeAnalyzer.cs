using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GodotSharpPlus.SourceGen;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class OnReadyNodeAttributeAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor _gdp0002Rule = new(
        "GDP0002",
        $"{OnReadyNodeAttributeInfo.Name} can only be used in classes derived from {NodeName}",
        $$"""Class '{0}' contains a field decorated with {{OnReadyNodeAttributeInfo.Name}} but is not derived from {{NodeName}}""",
        "Usage",
        DiagnosticSeverity.Error,
        true,
        customTags: WellKnownDiagnosticTags.NotConfigurable
    );
    private static readonly DiagnosticDescriptor _gdp0003Rule = new(
        "GDP0003",
        $"{OnReadyNodeAttributeInfo.Name} is only applicable to {NodeName} fields",
        $$"""Field '{0}' is decorated with {{OnReadyNodeAttributeInfo.Name}} but is not a {{NodeName}}""",
        "Usage",
        DiagnosticSeverity.Error,
        true,
        customTags: WellKnownDiagnosticTags.NotConfigurable
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
    [
        _gdp0002Rule,
        _gdp0003Rule,
    ];

    public override void Initialize(AnalysisContext initContext)
    {
        initContext.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        initContext.EnableConcurrentExecution();

        // ReSharper disable once ConvertIfStatementToSwitchStatement
        initContext.RegisterSyntaxNodeAction(static context =>
        {
            if (context.Node is ClassDeclarationSyntax classDeclarationSyntax)
            {
                if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not {IsNode: false} classSymbol) return;

                foreach (ISymbol symbol in classSymbol.GetMembers())
                {
                    if (symbol is IFieldSymbol fieldSymbol && fieldSymbol.GetAttributeData(OnReadyNodeAttributeInfo.QualifiedName) is not null)
                    {
                        Diagnostic diagnostic = Diagnostic.Create(_gdp0002Rule, classDeclarationSyntax.GetLocation(), classSymbol.Name);
                        context.ReportDiagnostic(diagnostic);
                        break;
                    }
                }
            }
            else if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax)
            {
                if (!fieldDeclarationSyntax.Declaration.Variables.Any() || context.SemanticModel.GetDeclaredSymbol(fieldDeclarationSyntax.Declaration.Variables[0]) is not IFieldSymbol {Type.IsNode: false, ContainingType.IsNode: true} fieldSymbol || fieldSymbol.GetAttributeData(OnReadyNodeAttributeInfo.QualifiedName) is null)
                {
                    return;
                }

                Diagnostic diagnostic = Diagnostic.Create(_gdp0003Rule, fieldDeclarationSyntax.GetLocation(), fieldSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }, SyntaxKind.ClassDeclaration, SyntaxKind.FieldDeclaration);
    }
}
