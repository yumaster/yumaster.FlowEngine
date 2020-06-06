using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace yumaster.FileService.WebApi.Swagger
{
    /// <summary>
    /// 截止Swashbuckle 1.0.0
    /// 
    /// 修复 IFormFile类型参数显示为undefined
    /// </summary>
    public class FixFormFileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var oParams = operation.Parameters;
            foreach (var aParam in context.ApiDescription.ParameterDescriptions)
            {
                if (typeof(IFormFile).IsAssignableFrom(aParam.Type))
                {
                    var pInfo = oParams.First(p => p.Name.Equals(aParam.Name, StringComparison.OrdinalIgnoreCase)) as NonBodyParameter;
                    if (pInfo != null)
                    {
                        pInfo.In = "formData";
                        pInfo.Type = "file";
                    }
                }
            }
        }
    }
}
