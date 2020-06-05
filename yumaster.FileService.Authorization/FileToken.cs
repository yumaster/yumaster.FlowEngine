using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Authorization
{
    /// <summary>
    /// 包含文件基本信息的文件访问令牌
    /// </summary>
    public class FileToken
    {
        /// <summary>
        /// 文件伪Id
        /// </summary>
        public uint PseudoId { get; set; }

        /// <summary>
        /// File表的Id字段
        /// </summary>
        public int FileId { get; set; }
        /// <summary>
        /// FileOwner表的Id字段
        /// </summary>
        public int FileOwnerId { get; set; }

        /// <summary>
        /// MIME类的Id字段
        /// </summary>
        public uint MimeId { get; set; }

        public DateTime FileCreateTime { get; set; }
        public DateTime ExpireTime { get; set; }

        public bool IsExpired => ExpireTime <= DateTime.Now;
    }
}
