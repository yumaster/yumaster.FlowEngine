using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace yumaster.FileService.Authorization.Utils
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public class FileUtil
    {
        public static bool TryDelete(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetSha1(string filePath)
        {
            using (FileStream inputStream = File.OpenRead(filePath))
            {
                return HashUtil.Sha1(inputStream);
            }
        }
    }
}
