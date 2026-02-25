// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator

using Microsoft.CodeAnalysis;

namespace GodotSharpPlus.SourceGen;

internal static class SymbolExtensions
{
    extension(ISymbol self)
    {
        public AttributeData? GetAttributeData(string qualifiedName)
        {
            foreach (AttributeData attributeData in self.GetAttributes())
            {
                if (attributeData.AttributeClass?.ToDisplayString() == qualifiedName) return attributeData;
            }
            return null;
        }
    }
}
