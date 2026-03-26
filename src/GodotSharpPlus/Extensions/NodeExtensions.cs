namespace GodotSharpPlus.Extensions;

/// <summary>
/// Extension members for <see cref="Node"/>.
/// </summary>
public static class NodeExtensions
{
    extension(Node self)
    {
        /// <summary>
        /// Deletes all children of this node.
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
        /// Queues all children of this node to be deleted at the end of the current frame.
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
        /// Removes all children of this node. The children are <b>not</b> deleted; see
        /// <see cref="QueueFreeChildren"/>.
        /// </summary>
        [PublicAPI]
        public void RemoveAllChildren()
        {
            foreach (Node child in self.GetChildren()) self.RemoveChild(child);
        }

        /// <summary>
        /// Adds the given <paramref name="scene"/> as a child node.
        /// </summary>
        /// <param name="scene">
        /// The <see cref="PackedScene"/> to instantiate.
        /// </param>
        /// <param name="editState">
        /// Mainly relevant to the editor.
        /// </param>
        /// <param name="forceReadableName">
        /// If <c>true</c>, improves the readability of the instantiated scene's name.
        /// </param>
        /// <param name="internal">
        /// If not <see cref="Node.InternalMode.Disabled"/>, the instantiated scene is
        /// internal.
        /// </param>
        [PublicAPI]
        public void AddSceneChild(PackedScene scene, PackedScene.GenEditState editState = PackedScene.GenEditState.Disabled, bool forceReadableName = false, Node.InternalMode @internal = Node.InternalMode.Disabled) => scene.InstantiateAsChild<Node>(self, editState, forceReadableName, @internal);
    }
}
