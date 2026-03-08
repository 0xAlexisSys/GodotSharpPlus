namespace GodotSharpPlus.Attributes;

/// <summary>
/// A <see cref="Node"/> field decorated with this attribute is included in
/// <b>generated</b> private method <c>InitNodePaths</c>, which should be called
/// in <see cref="Node._Ready"/>.
/// </summary>
/// <param name="path">
/// Path to a <see cref="Node"/> in the scene tree.
/// </param>
[PublicAPI, AttributeUsage(AttributeTargets.Field)]
public sealed class OnReadyNodeAttribute(string path) : Attribute
{
    public readonly string Path = path;
}
