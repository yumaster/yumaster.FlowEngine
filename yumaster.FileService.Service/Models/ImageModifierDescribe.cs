using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Service.Models
{
    /// <summary>
    /// 图像转换修饰信息描述
    /// </summary>
    public class ImageModifierDescribe : IModifierDescribe
    {
        public string Syntax => "[site]_[format]";
        public string[] Sizes { get; set; }
        public string[] Formats { get; set; }
    }
}
