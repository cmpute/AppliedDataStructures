namespace System.Collections.Advanced
{
    /// <summary>
    /// Indicating traversing order of the tree
    /// 用来标志遍历（二叉）树时的顺序
    /// </summary>
    public enum TraverseOrder
    {
        /********** Depth First (DFS) **********/

        /// <summary>
        /// 前序遍历
        /// </summary>
        /// <remarks>Binary Tree only.</remarks>
        PreOrder,
        /// <summary>
        /// 中序遍历
        /// </summary>
        InOrder,
        /// <summary>
        /// 后序遍历
        /// </summary>
        /// <remarks>Binary Tree only.</remarks>
        PostOrder,

        /********** Breadth First (BFS) **********/

        /// <summary>
        /// 层次遍历
        /// </summary>
        LevelOrder
    }
}