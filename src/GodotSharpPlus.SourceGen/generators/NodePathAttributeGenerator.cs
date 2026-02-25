// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GodotSharpPlus.SourceGen;

internal static class NodePathAttributeInfo
{
    public const string Name = "NodePathAttribute";
    public const string QualifiedName = $"{AttributeNamespace}.{Name}";
}

[Generator]
internal sealed class NodePathAttributeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        IncrementalValuesProvider<ClassData> provider = initContext.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax {BaseList: not null},
            static (context, _) =>
            {
                // ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
                ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
                if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol {IsNode: true} classSymbol) return new ClassData();

                Dictionary<IFieldSymbol, string> nodeFields = [];
                foreach (ISymbol symbol in classSymbol.GetMembers())
                {
                    if (symbol is IFieldSymbol {Type.IsNode: true} fieldSymbol && fieldSymbol.GetAttributeData(NodePathAttributeInfo.QualifiedName) is {} attributeData)
                    {
                        nodeFields.Add(fieldSymbol, attributeData.ConstructorArguments[0].Value?.ToString() ?? string.Empty);
                    }
                }
                return new ClassData(classDeclarationSyntax, classSymbol, nodeFields);
                // ReSharper restore ArrangeObjectCreationWhenTypeNotEvident
            }
        ).Where(static classData => classData.DeclarationSyntax is not null && classData.Symbol is not null && classData.NodeFields.Count != 0);

        initContext.RegisterSourceOutput(provider, static (context, classData) =>
        {
            string className = classData.DeclarationSyntax!.Identifier.Text;

            StringBuilder sourceCodeBuilder = new($$"""
                                                    {{SourceCodeHeader}}

                                                    namespace {{classData.Symbol!.ContainingNamespace.ToDisplayString()}};

                                                    partial class {{className}}
                                                    {
                                                        private void InitNodes()
                                                        {

                                                    """);
            sourceCodeBuilder.EnsureCapacity(640);
            foreach (KeyValuePair<IFieldSymbol, string> pair in classData.NodeFields)
            {
                string typeQualifiedName = pair.Key.Type.ToDisplayString().TrimEnd('?');

                sourceCodeBuilder.Append($"        {pair.Key.Name} = GetNode");
                if (typeQualifiedName != NodeQualifiedName) sourceCodeBuilder.Append($"<{typeQualifiedName}>");
                sourceCodeBuilder.AppendLine($"(\"{pair.Value}\");");
            }
            sourceCodeBuilder.Append("    }\n}");

            context.AddSource(className + SourceFileNameSuffix, SourceText.From(sourceCodeBuilder.ToString(), SourceFileEncoding));
        });
    }

    private readonly record struct ClassData(ClassDeclarationSyntax? DeclarationSyntax, INamedTypeSymbol? Symbol, Dictionary<IFieldSymbol, string> NodeFields);
}
