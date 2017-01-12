﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Multiway Tree
    /// 多路树
    /// </summary>
    public interface IMultiwayTreeNode
    {
        /// <summary>
        /// 当前结点的孩子列表
        /// </summary>
        IReadOnlyList<IMultiwayTreeNode> Children { get; }
    }
}
