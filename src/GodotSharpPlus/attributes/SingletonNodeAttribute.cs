namespace GodotSharpPlus.Attributes;

/// <summary>
/// A <see cref="Node"/>-derived class decorated with this attribute can have only
/// one instance in the scene tree at a time.
/// </summary>
/// <remarks>
/// This attribute implements a partial constructor which must be defined in the
/// class. Any code that typically goes into the constructor should be put in an
/// implementation of <b>generated</b> partial method <c>Init</c>.
/// </remarks>
[PublicAPI, AttributeUsage(AttributeTargets.Class)]
public sealed class SingletonNodeAttribute : Attribute;
