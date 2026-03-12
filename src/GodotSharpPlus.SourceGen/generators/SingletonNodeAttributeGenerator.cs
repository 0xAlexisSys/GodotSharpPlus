// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

using System.Text;
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
    private static readonly Dictionary<Accessibility, string> _constructorAccessibilityMap = new()
    {
        [Accessibility.NotApplicable] = string.Empty,
        [Accessibility.Public] = "public",
        [Accessibility.Protected] = "protected",
        [Accessibility.Private] = "private",
        [Accessibility.Internal] = "internal",
        [Accessibility.ProtectedOrInternal] = "protected internal",
        [Accessibility.ProtectedAndInternal] = "private protected",
    };

    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        IncrementalValuesProvider<ClassData> provider = initContext.SyntaxProvider.ForAttributeWithMetadataName(
            SingletonNodeAttributeInfo.QualifiedName,
            static (node, _) => node is ClassDeclarationSyntax {BaseList: not null},
            static (context, _) =>
            {
                // ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
                ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.TargetNode;
                return context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is INamedTypeSymbol {IsNode: true} classSymbol && classSymbol.GetAttributeData(SingletonNodeAttributeInfo.QualifiedName) is not null ? new ClassData(classDeclarationSyntax, classSymbol) : new ClassData();
                // ReSharper restore ArrangeObjectCreationWhenTypeNotEvident
            }
        ).Where(static classData => classData.DeclarationSyntax is not null && classData.Symbol is not null);

        initContext.RegisterSourceOutput(provider, static (context, classData) =>
        {
            string className = classData.DeclarationSyntax!.Identifier.Text;

            StringBuilder sourceCodeBuilder = StringBuilder.InitSourceCode(640, classData.Symbol!);
            sourceCodeBuilder.AppendLine($$"""
                                           partial class {{className}}
                                           {
                                               public static {{className}}? Instance { get; private set; }

                                               #nullable disable
                                           """);
            foreach (IMethodSymbol constructorSymbol in classData.Symbol!.Constructors)
            {
                if (!constructorSymbol.IsStatic)
                {
                    sourceCodeBuilder.AppendLine($$"""
                                                       {{_constructorAccessibilityMap[constructorSymbol.DeclaredAccessibility]}} partial {{className}}()
                                                       {
                                                           if ({{GodotNamespace}}.GodotObject.IsInstanceValid(Instance)) throw new System.InvalidOperationException("An instance of {{className}} already exists.");

                                                           Instance = this;
                                                           Init();
                                                       }
                                                   """);
                }
            }
            sourceCodeBuilder.Append("""
                                         #nullable restore

                                         partial void Init();
                                     }
                                     """);

            context.AddSource(className + SourceFileNameSuffix, SourceText.From(sourceCodeBuilder.ToString(), SourceFileEncoding));
        });
    }

    private readonly record struct ClassData(ClassDeclarationSyntax? DeclarationSyntax, INamedTypeSymbol? Symbol);
}
