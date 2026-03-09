namespace GodotSharpPlus.Extensions;

/// <summary>
/// Extension members for <see cref="PackedScene"/>.
/// </summary>
public static class PackedSceneExtensions
{
    extension(PackedScene self)
    {
        /// <summary>
        /// Instantiates the scene as a child of the given <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">
        /// The <see cref="Node"/> that will add the instantiated scene.
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
        /// <typeparam name="T">
        /// The type to cast to. Should be <see cref="Node"/> or inherit from it.
        /// </typeparam>
        /// <returns>
        /// The instantiated scene.
        /// </returns>
        [PublicAPI]
        public T InstantiateAsChild<T>(Node parent, PackedScene.GenEditState editState = PackedScene.GenEditState.Disabled, bool forceReadableName = false, Node.InternalMode @internal = Node.InternalMode.Disabled) where T : Node
        {
            T node = self.Instantiate<T>(editState);
            parent.AddChild(node, forceReadableName, @internal);
            return node;
        }

        /// <summary>
        /// Attempts to instantiate the scene as a child of the given
        /// <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">
        /// The <see cref="Node"/> that will add the instantiated scene.
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
        /// <typeparam name="T">
        /// The type to cast to. Should be <see cref="Node"/> or inherit from it.
        /// </typeparam>
        /// <returns>
        /// The instantiated scene; <c>null</c> if the instantiation failed.
        /// </returns>
        [PublicAPI]
        public T? TryInstantiateAsChild<T>(Node parent, PackedScene.GenEditState editState = PackedScene.GenEditState.Disabled, bool forceReadableName = false, Node.InternalMode @internal = Node.InternalMode.Disabled) where T : Node
        {
            T? node = self.InstantiateOrNull<T>(editState);
            if (node is not null) parent.AddChild(node, forceReadableName, @internal);
            return node;
        }
    }
}
