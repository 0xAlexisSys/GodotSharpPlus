using Microsoft.CodeAnalysis;

namespace GodotSharpPlus.SourceGen;

internal static class TypeSymbolExtensions
{
    extension(ITypeSymbol self)
    {
        public bool IsNode
        {
            get
            {
                if (self.ToDisplayString() == NodeQualifiedName) return true;

                INamedTypeSymbol? typeSymbol = self.BaseType;
                while (typeSymbol is not null)
                {
                    if (typeSymbol.ToDisplayString() == NodeQualifiedName) return true;
                    typeSymbol = typeSymbol.BaseType;
                }
                return false;
            }
        }
    }
}
