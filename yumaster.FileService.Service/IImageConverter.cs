using System.Threading.Tasks;
using yumaster.FileService.Service.Models;

namespace yumaster.FileService.Service
{
    /// <summary>
    /// 图像转换器
    /// </summary>
    public interface IImageConverter
    {
        Task ConvertAsync(string srcFilePath, Mime srcMime, string dstFilePath, ImageModifier dstImgMod);
    }
}
