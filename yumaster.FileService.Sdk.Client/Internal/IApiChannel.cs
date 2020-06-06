using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace yumaster.FileService.Sdk.Client.Internal
{
    /// <summary>
    /// API传输通道接口定义
    /// </summary>
    internal interface IApiChannel : IDisposable
    {
        IWebProxy Proxy { get; set; }

        /// <summary>
        /// 调用API并获取返回结果的流
        /// </summary>
        Task<TextReader> GetTextReaderAsync(HttpMethod method, string apiPath, HttpContent reqContent = null);
    }
}
