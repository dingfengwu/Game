using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Base
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T> 
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.RecordFirstNumber = Math.Min(this.TotalCount, pageIndex * pageSize + 1);
            this.RecordEndNumber = Math.Min(this.TotalCount, (pageIndex + 1) * pageSize);
            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.RecordFirstNumber = Math.Min(this.TotalCount, pageIndex * pageSize + 1);
            this.RecordEndNumber = Math.Min(this.TotalCount, (pageIndex + 1) * pageSize);
            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.RecordFirstNumber = Math.Min(this.TotalCount, pageIndex * pageSize + 1);
            this.RecordEndNumber = Math.Min(this.TotalCount, (pageIndex + 1) * pageSize);
            this.AddRange(source);
        }

        /// <summary>
        /// Page index
        /// </summary>
        public int PageIndex { get; }
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; }
        /// <summary>
        /// Total count
        /// </summary>
        public int TotalCount { get; }
        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages { get; }
        /// <summary>
        /// Has previous page
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        /// <summary>
        /// Has next page
        /// </summary>
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }

        /// <summary>
        /// 起始记录序号
        /// </summary>
        public int RecordFirstNumber { get; }

        /// <summary>
        /// 终止记录序号
        /// </summary>
        public int RecordEndNumber { get; }

        /// <summary>
        /// 当前页的记录条数
        /// </summary>
        public int CurrentPageRecordCount => Math.Min(TotalCount, (RecordEndNumber - RecordFirstNumber + 1));
    }
}
