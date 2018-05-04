using System.Collections.Generic;

namespace Game.Base
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// Page index
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// Page size
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// Total count
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// Total pages
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// Has previous page
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// Has next age
        /// </summary>
        bool HasNextPage { get; }
        /// <summary>
        /// 当前记录的起始序号,序号从1开始
        /// </summary>
        int RecordFirstNumber { get; }
        /// <summary>
        /// 当前记录的终止序号,序号从1开始
        /// </summary>
        int RecordEndNumber { get;}
        /// <summary>
        /// 当前页显示的记录条数
        /// </summary>
        int CurrentPageRecordCount { get;}
    }
}
