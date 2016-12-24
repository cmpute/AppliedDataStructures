using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class BinarySearchTreeNode<TKey, TData> : BinaryTreeNode, IComparableNode<TKey>
    {
        public virtual TKey Key { get; set; }
        public virtual TData Data { get; set; }
        /// <summary>
        /// Swap key and data with certain node
        /// 与指定结点交换节点数据
        /// </summary>
        /// <param name="target">需要交换的结点</param>
        public void SwapWith(BinarySearchTreeNode<TKey, TData> target)
        {
            OnSearchDown();
            target.OnSearchDown();
            var tkey = Key;
            Key = target.Key;
            target.Key = tkey;
            var tdata = Data;
            Data = target.Data;
            target.Data = tdata;
            Parent?.OnSearchUp();
            target.Parent?.OnSearchUp();
        }
    }
}
