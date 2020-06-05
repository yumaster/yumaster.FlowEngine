using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Authorization
{
    /// <summary>
    /// 包含文件所有者身份信息的访问令牌
    /// </summary>
    public class OwnerToken
    {
        public int OwnerType { get; set; }
        public int OwnerId { get; set; }
        public DateTime ExpireTime { get; set; }

        public bool IsExpired => ExpireTime <= DateTime.Now;
    }
}
