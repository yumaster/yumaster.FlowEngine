using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using yumaster.FileService.Db.Entities;

namespace yumaster.FileService.WebApi.Models.Input.Server
{
    public class UploadFileInput : FileOwnerTypeId
    {
        /// <summary>
        /// 文件名（包含扩展名），不传则从文件流中读取。例如：test.jpg
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 待上传的文件流
        /// </summary>
        public IFormFile File { get; set; }
        /// <summary>
        /// 待上传文件的SHA1值。例如：c64376a0d4677f0d4df563fe23f0c8239a45c17d
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 链接有效期（分钟）,0则不过期
        /// </summary>
        [DefaultValue(0)]
        public int PeriodMinute { get; set; } = 0;
    }

    public class UploadFileByRemoteInput : FileOwnerTypeId
    {
        /// <summary>
        /// 文件名（包含扩展名），不传则从文件流中读取。例如：test.jpg
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件下载地址
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// 链接有效期（分钟）,0则不过期
        /// </summary>
        [DefaultValue(0)]
        public int PeriodMinute { get; set; } = 0;
    }
}
