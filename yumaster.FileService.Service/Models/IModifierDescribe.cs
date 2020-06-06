using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Service.Models
{
    /// <summary>
    /// 转换修饰信息描述
    /// </summary>
    public interface IModifierDescribe
    {
        /// <summary>
        /// 参数拼接语法，参数间用_分隔
        /// </summary>
        string Syntax { get; }
    }
}
