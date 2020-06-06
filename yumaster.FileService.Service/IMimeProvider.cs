using System;
using System.Collections.Generic;
using System.Text;
using yumaster.FileService.Service.Models;

namespace yumaster.FileService.Service
{
    public interface IMimeProvider
    {
        Mime GetMimeByExtensionName(string extName);
        Mime GetMimeByFile(string filePath, string extName);
        Mime GetMimeById(uint id);
        IEnumerable<string> ImageExtensionNames { get; }
    }
}
