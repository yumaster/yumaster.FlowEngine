using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace yumaster.FileService.WebApi.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private volatile ILogger logger;
        protected ILogger Logger
        {
            get
            {
                if(logger==null)
                {
                    var resolver = HttpContext.RequestServices;
                    if (resolver == null)
                        throw new InvalidOperationException($"{nameof(HttpContext.RequestServices)} is null");

                    logger = resolver.GetRequiredService<ILoggerFactory>().CreateLogger(GetType().Name);
                }
                return logger;
            }
        }
        public IServiceProvider RequestService => HttpContext.RequestServices;
        public ISession Session => HttpContext.Session;
    }
}