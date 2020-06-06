using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using yumaster.FileService.Service.Models.Result;
using yumaster.FileService.WebApi.Options;

namespace yumaster.FileService.WebApi.Controllers
{
    public abstract class BaseServerApiController : ControllerBase
    {
        private readonly IOptionsMonitor<ManageOption> _manageOption;

        protected BaseServerApiController(IOptionsMonitor<ManageOption> manageOption)
        {
            _manageOption = manageOption;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var connInfo = context.HttpContext.Connection;
            var cIpAddr = connInfo.RemoteIpAddress.ToString();

            var mgrOpt = _manageOption.CurrentValue;
            if (!mgrOpt.IsLocalIp(cIpAddr) && !mgrOpt.IpWhitelist.Any(p => p == "*" || p == cIpAddr))
                context.Result = new JsonResult(new Result(ResultErrorCodes.Unauthorized, $"您的IP({cIpAddr})没有权限访问"));
            else
                base.OnActionExecuting(context);
        }
    }
}