namespace System.Collections.Advanced
{
    /// <summary>
    /// Indicating traversing order of the tree
    /// 用来标志遍历（二叉）树时的顺序
    /// </summary>
    public enum TraversalOrder
    {
        /// <summary>
        /// 前序遍历
        /// </summary>
        PreOrder,
        /// <summary>
        /// 中序遍历
        /// </summary>
        InOrder,
        /// <summary>
        /// 后序遍历
        /// </summary>
        PostOrder,
        /// <summary>
        /// 层次遍历
        /// </summary>
        LevelOrder
    }
}