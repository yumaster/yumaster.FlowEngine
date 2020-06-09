using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using yumaster.FileService.Authorization;
using yumaster.FileService.Authorization.Codecs;
using yumaster.FileService.Db.Options;
using yumaster.FileService.Service;
using yumaster.FileService.Service.Options;
using yumaster.FileService.Service.ServiceImpls;
using yumaster.FileService.WebApi.AutoReview;
using yumaster.FileService.WebApi.Extensions;
using yumaster.FileService.WebApi.Filters;
using yumaster.FileService.WebApi.Options;

namespace yumaster.FileService.WebApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _cfg;
        private readonly string apiName = "文件服务";
        public Startup(IConfiguration cfg, IHostingEnvironment env)
        {
            _cfg = cfg;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            ServiceConfigure.AddAuthorization(services, opt =>
             {
                 opt.AppSecret = _cfg["General:AppSecret"];
             });
            services.AddSingleton<IFileTokenCodec, FileTokenCodec>();

            services.AddSingleton<IMimeProvider, MimeProvider>();
            services.AddSingleton<ImageSizeProvider>();
            services.AddSingleton<RawFileHandler>();
            services.AddSingleton<ImageFileHandler>();
            services.AddSingleton(svces => new FileHandlerManager(svces));

            //注册内部服务
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //选项
            services.AddOptions();
            services.Configure<ServerOption>(_cfg.GetSection("Server"));
            services.Configure<GeneralOption>(_cfg.GetSection("General"));
            services.Configure<ImageConverterOption>(_cfg.GetSection("ImageConverter"));
            services.Configure<DbOption>(_cfg.GetSection("Db"));
            services.Configure<ManageOption>(_cfg.GetSection("Manage"));
            services.Configure<ClusterOption>(_cfg.GetSection("Cluster"));
            #region UEditor
            services.AddSingleton<UEditorOption>();
            #endregion
            services.AddMvc(opt =>
            {
                opt.Filters.Add<ValidateModelAttribute>();
            }).AddNewtonsoftJson(opt =>
            {
                var setts = opt.SerializerSettings;
                setts.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                setts.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                setts.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";
            });
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAny", b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            });
            #region swagger
            //if (_env.IsDevelopment())
            //    services.AddSwaggerService(PlatformServices.Default.Application.ApplicationBasePath);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",//版本号
                    Title = $"{apiName}接口文档",
                    Description = $"{apiName}HTTP API V1",//编辑描述
                    Contact = new OpenApiContact { Name = apiName, Email = "yumaster@yeah.net" },//联系方式
                    License = new OpenApiLicense { Name = apiName }//编辑许可证
                });
                c.OrderActionsBy(o => o.RelativePath);
                var xmlPath = Path.Combine(Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath, "yumaster.FileService.WebApi.xml");// 配置接口文档文件路径
                c.IncludeXmlComments(xmlPath, true); // 把接口文档的路径配置进去。第二个参数表示的是是否开启包含对Controller的注释容纳
            });
            #endregion


            //确保服务依赖的正确性，放到所有注册服务代码后调用
            if (_env.IsDevelopment())
            {
                services.AddAutoReview(
                    new DependencyInjectionAssert()
                    {
                        IgnoreTypes = new[]
                        {
                            "Microsoft.AspNetCore.Mvc.Razor.Internal.TagHelperComponentManager",
                            "Microsoft.Extensions.DependencyInjection.IServiceScopeFactory"
                        }
                    }
                );
            }

        }
#if DEBUG_
            var serviceList = services.Dump();
            System.Diagnostics.Debugger.Break();
#endif


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwaggerService();
            }else
            {
                app.UseExceptionHandler(new GlobalExceptionHandlerOptions());
            }
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"{apiName} v1");
                c.RoutePrefix = "";
            });
            #endregion
            app.UseRouting();

            app.UseStaticFiles();
            app.UseStatusCodePages("text/html", "<div>There is a problem with the page you're visiting, StatusCode: {0}</div>");
        }
    }
}
