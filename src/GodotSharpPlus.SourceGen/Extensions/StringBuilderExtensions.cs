using System.Text;
using Microsoft.CodeAnalysis;

namespace GodotSharpPlus.SourceGen.Extensions;

internal static class StringBuilderExtensions
{
    extension(StringBuilder)
    {
        public static StringBuilder InitSourceCode(s32 capacity, INamedTypeSymbol namedTypeSymbol)
        {
            StringBuilder sourceCodeBuilder = new($"""
                                                   {SourceCodeHeader}


                                                   """);
            sourceCodeBuilder.EnsureCapacity(capacity);
            if (!namedTypeSymbol.ContainingNamespace.IsGlobalNamespace)
            {
                sourceCodeBuilder.AppendLine($"""
                                              namespace {namedTypeSymbol.ContainingNamespace.ToDisplayString()};

                                              """);
            }
            return sourceCodeBuilder;
        }
    }
}
