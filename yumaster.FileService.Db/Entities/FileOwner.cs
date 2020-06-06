using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Db.Entities
{
    /// <summary>
    /// 文件所有者信息
    /// </summary>
    public class FileOwner : FileOwnerTypeId
    {
        public int Id { get; set; }
        /// <summary>
        /// File表的ID引用，假设FileOwner与File表号一一对应
        /// </summary>
        public int FileId { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class FileOwnerTypeId
    {
        public int OwnerType { get; set; }
        public int OwnerId { get; set; }
    }
}
