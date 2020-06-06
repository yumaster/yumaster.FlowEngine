using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using yumaster.FileService.Service.Models.Result;
using yumaster.FileService.WebApi.Extensions;

namespace yumaster.FileService.WebApi.Filters
{
    /// <summary>
    /// 确保当前Action的Model是已验证的，否则返回错误响应结果
    /// </summary>
    public class ValidateModelAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new Result(ResultErrorCodes.ArgumentBad, context.ModelState.GetFirstErrorMessage());
                context.Result = new JsonResult(result);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
