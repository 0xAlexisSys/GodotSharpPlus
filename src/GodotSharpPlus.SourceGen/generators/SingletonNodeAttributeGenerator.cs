using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GodotSharpPlus.SourceGen;

internal static class SingletonNodeAttributeInfo
{
    public const string Name = "SingletonNodeAttribute";
    public const string QualifiedName = $"{AttributeNamespace}.{Name}";
}

[Generator]
internal sealed class SingletonNodeAttributeGenerator : IIncrementalGenerator
{
    private static readonly string[] _constructorAccessibilityMap =
    [
        "public",
        "protected",
        "private",
    ];

    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        IncrementalValuesProvider<ClassData> provider = initContext.SyntaxProvider.ForAttributeWithMetadataName(
            SingletonNodeAttributeInfo.QualifiedName,
            static (node, _) => node is ClassDeclarationSyntax {BaseList: not null},
            static (context, _) =>
            {
                // ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
                ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.TargetNode;

                if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol {IsNode: true} classSymbol || classSymbol.GetAttributeData(SingletonNodeAttributeInfo.QualifiedName) is not {} attributeData)
                {
                    return new ClassData();
                }
                return new ClassData(classDeclarationSyntax, classSymbol, Convert.ToByte(attributeData.ConstructorArguments[0].Value));
                // ReSharper restore ArrangeObjectCreationWhenTypeNotEvident
            }
        ).Where(static classData => classData.DeclarationSyntax is not null && classData.Symbol is not null);

        initContext.RegisterSourceOutput(provider, static (context, classData) =>
        {
            string className = classData.DeclarationSyntax!.Identifier.Text;
            string sourceCode = $$"""
                                  {{SourceCodeHeader}}

                                  namespace {{classData.Symbol!.ContainingNamespace.ToDisplayString()}};

                                  partial class {{className}}
                                  {
                                      public static {{className}}? Instance { get; private set; }

                                      #nullable disable
                                      {{_constructorAccessibilityMap[classData.ConstructorAccessibility]}} {{className}}()
                                      {
                                          if ({{className}}.IsInstanceValid(Instance)) throw new System.InvalidOperationException("An instance of {{className}} already exists.");

                                          Instance = this;
                                          Init();
                                      }
                                      #nullable restore

                                      partial void Init();
                                  }
                                  """;

            context.AddSource(className + SourceFileNameSuffix, SourceText.From(sourceCode, SourceFileEncoding));
        });
    }

    private readonly record struct ClassData(ClassDeclarationSyntax? DeclarationSyntax, INamedTypeSymbol? Symbol, u8 ConstructorAccessibility);
}
