using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    /// <summary>
    /// Abstraction of (Partially) Persistent Data Structure
    /// (部分)可持久化数据结构的抽象接口
    /// </summary>
    public interface IPersistent
    {
        /// <summary>
        /// Version of current structure. Can be seen as a timestamp
        /// 当前结构的版本，可以看作时间戳
        /// </summary>
        int Version { get; }
        /// <summary>
        /// Revert structure to certain version
        /// 将数据结构回滚至某一版本
        /// </summary>
        /// <param name="targetVersion"></param>
        //void Revert(int targetVersion);
        /// <summary>
        /// Get or set whether to record previous version of the structure while operating
        /// 获取或设置操作时是否记录以前的版本
        /// </summary>
        /// <remarks>
        /// If the property is set as false, then the structure is not persistent, so as to save space.
        /// 如果属性设为false，那么该结构就不是可持久化的以节省空间
        /// </remarks>
        //bool Record { get; set; }
    }
}
