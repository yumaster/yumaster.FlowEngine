using yumaster.FileService.Authorization.Options;

namespace yumaster.FileService.Sdk.Server
{
    public class FileServiceOption : AuthOption
    {
        /// <summary>
        /// 服务服务器IP地址
        /// </summary>
        public string Host { get; set; }
    }
}
