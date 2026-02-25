namespace GodotSharpPlus.Extensions;

/// <summary>
/// Extension members for <see cref="Node"/>.
/// </summary>
public static class NodeExtensions
{
    extension(Node self)
    {
        /// <summary>
        /// Deletes all children of this <see cref="Node"/>.
        /// </summary>
        /// <param name="includeInternal">
        /// If <c>true</c>, internal children are also deleted.
        /// </param>
        [PublicAPI]
        public void FreeChildren(bool includeInternal = false)
        {
            foreach (Node child in self.GetChildren(includeInternal)) child.Free();
        }

        /// <summary>
        /// Queues all children of this <see cref="Node"/> to be deleted at the end of the current frame.
        /// </summary>
        /// <param name="includeInternal">
        /// If <c>true</c>, internal children are also queued for deletion.
        /// </param>
        [PublicAPI]
        public void QueueFreeChildren(bool includeInternal = false)
        {
            foreach (Node child in self.GetChildren(includeInternal)) child.QueueFree();
        }

        /// <summary>
        /// Removes all children of this <see cref="Node"/>. The children are <b>not</b> deleted; see
        /// <see cref="QueueFreeChildren"/>.
        /// </summary>
        [PublicAPI]
        public void RemoveAllChildren()
        {
            foreach (Node child in self.GetChildren()) self.RemoveChild(child);
        }
    }
}
