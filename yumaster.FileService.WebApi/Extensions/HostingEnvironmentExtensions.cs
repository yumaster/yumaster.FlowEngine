using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace yumaster.FileService.WebApi.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// 获取配置文件根目录
        /// </summary>
        public static string GetConfigPath(this IHostingEnvironment env)
        {
            return Path.Combine(env.ContentRootPath, "confs");
        }
    }
}
