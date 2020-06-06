using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace yumaster.FileService.WebApi.Swagger
{
    /// <summary>
    /// 截止Swashbuckle 2.2.0
    /// 
    /// 修复 调用接口时请求Content-Type为undefined
    /// </summary>
    public class FixContentTypeOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Consumes = new[] { "application/x-www-form-urlencoded" };
        }
    }
}
