using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Sdk.Client.Models
{
    /// <summary>
    /// 带分页的列表数据
    /// </summary>
    public class PagedItemsData<T> : ItemsData<T>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPages { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalItems { get; set; }
    }
}
