namespace GodotSharpPlus.Extensions;

/// <summary>
/// Extension members for <see cref="CanvasItem"/>.
/// </summary>
public static class CanvasItemExtensions
{
    extension(CanvasItem self)
    {
        /// <summary>
        /// Returns the cursor's global position (converted to <see cref="Vector2I"/>)
        /// relative to the <see cref="CanvasLayer"/> that contains this node.
        /// </summary>
        /// <returns>
        /// The cursor's relative global position.
        /// </returns>
        [PublicAPI]
        public Vector2I GetGlobalMousePositionI() => (Vector2I)self.GetGlobalMousePosition();
    }
}
