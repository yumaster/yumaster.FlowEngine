using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Db.Entities
{
    /// <summary>
    /// 文件所有者配额信息
    /// </summary>
    public class OwnerQuota
    {
        public long Id { get; set; }
        public int OwnerType { get; set; }
        public long OwnerId { get; set; }
        public long Used { get; set; }
        public long Max { get; set; }
    }
}
