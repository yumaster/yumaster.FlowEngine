using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace yumaster.FileService.WebApi.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// 获取第1个错误消息
        /// </summary>
        public static string GetFirstErrorMessage(this ModelStateDictionary modelState)
        {
            return modelState.Values
                .First(p => p.ValidationState == ModelValidationState.Invalid)
                .Errors.First()
                .ErrorMessage;
        }
    }
}
