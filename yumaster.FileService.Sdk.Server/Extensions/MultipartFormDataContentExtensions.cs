using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace yumaster.FileService.Sdk.Server.Extensions
{
    /// <summary>
    /// MultipartFormDataContent的扩展方法
    /// </summary>
    public static class MultipartFormDataContentExtensions
    {
        /// <summary>
        /// 添加字符串字段
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public static void AddByString(this MultipartFormDataContent this_, string name, string content)
        {
            this_.Add(new StringContent(content), name);
        }
        /// <summary>
        /// 添加文件字段
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        public static void AddByFile(this MultipartFormDataContent this_, string name, string filePath)
        {
            FileStream content = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            string fileName = Path.GetFileName(filePath);
            StreamContent streamContent = new StreamContent(content);
            streamContent.Headers.ContentType = GetContentTypeByFile(fileName);
            this_.Add(streamContent, name, fileName);
        }

        private static MediaTypeHeaderValue GetContentTypeByFile(string fileName)
        {
            string mediaType = "application/octet-stream";
            string text = string.IsNullOrEmpty(fileName) ? null : Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Substring(1).ToLower();
                switch (text)
                {
                    case "jpg":
                        mediaType = "image/jpeg";
                        break;
                    case "bmp":
                    case "png":
                    case "gif":
                    case "jpeg":
                        mediaType = "image/" + text;
                        break;
                    case "txt":
                        mediaType = "text/plain";
                        break;
                    case "avi":
                    case "asf":
                    case "wmv":
                    case "mp4":
                    case "rm":
                    case "rmvb":
                    case "3gp":
                    case "mpg":
                    case "mov":
                    case "flv":
                        mediaType = "video/" + text;
                        break;
                    case "wma":
                    case "mp3":
                    case "wav":
                        mediaType = "audio/" + text;
                        break;
                }
            }
            return new MediaTypeHeaderValue(mediaType);
        }
    }
}
