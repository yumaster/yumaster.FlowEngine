using System.Collections.Generic;

namespace yumaster.FileService.Sdk.Server.Models.Output
{
    public class ListData<T>
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<T> List { get; set; }
    }
}
