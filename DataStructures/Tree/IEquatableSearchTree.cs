using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public interface IStaticEquatableSearchTree<out TNode, in TKey> : IStaticSearchTree<TNode, TKey>
        where TNode : IKeyProvider<TKey>
    {
        IEnumerable<TNode> SearchNodeAll(TKey key);
    }
    public interface IEquatableSearchTree<TNode, in TKey> : IStaticEquatableSearchTree<TNode, TKey>, ISearchTree<TNode, TKey>
        where TNode : IKeyProvider<TKey>
    {
        IEnumerable<TNode> DeleteNodeAll(TKey key);
        /// <summary>
        /// 表示树是否支持<see cref="ISearchTreeEquatable{TNode, TKey}"/>接口包含的操作
        /// </summary>
        /// <remarks>
        /// 由于<see cref="ISearchTreeEquatable{TNode, TKey}"/>继承了<see cref="ISearchTree{TNode, TKey}"/>，该属性用于转换支持和不支持相同元素的模式
        /// 不支持相同元素的操作时可以用作集合(Set)
        /// </remarks>
        bool SupportEquatable { set; }
    }
}
