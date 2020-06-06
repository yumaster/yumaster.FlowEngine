using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Service.Models.Result
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfo
    {
        public string Name { get; set; }
        public long Length { get; set; }
        public string Sign { get; set; }
    }
}
