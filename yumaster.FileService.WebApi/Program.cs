using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using yumaster.FileService.WebApi.Extensions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace yumaster.FileService.WebApi
{
    public class Program
    {
        private static ILogger logger;
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            try
            {
                var webHost = CreateHostBuilder(args).Build();
                var hostEnv = webHost.Services.GetRequiredService<IHostingEnvironment>();
                if (!Directory.Exists(hostEnv.GetConfigPath()))
                {
                    Console.WriteLine("Unable to detect the confs path");
                    return;
                }
                //logger
                var logFac = webHost.Services.GetRequiredService<ILoggerFactory>();
                logger = logFac.CreateLogger<Program>();
                webHost.Run();
                System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostCtx, cfg) =>
                    {
                        cfg.AddEnvironmentVariables();
                        if (args != null)
                            cfg.AddCommandLine(args);
                        cfg.SetBasePath(Path.Combine(hostCtx.HostingEnvironment.ContentRootPath, "confs"));
                        var env = hostCtx.HostingEnvironment as IHostingEnvironment;
                        cfg.SetBasePath(env.GetConfigPath());
                        cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                        cfg.AddJsonFile("image-sizes.json", optional: false, reloadOnChange: true);
                        cfg.AddJsonFile("mimes.json", optional: false, reloadOnChange: true);
                        if (env.IsDevelopment())
                        {
                            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                            if (appAssembly != null)
                                cfg.AddUserSecrets(appAssembly, optional: true);
                        }
                    })
                     .ConfigureLogging((ctx, logging) =>
                     {
                         logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                         logging.AddConsole();
                         logging.AddDebug();
                         logging.AddNLog(ctx.HostingEnvironment as IHostingEnvironment, ctx.Configuration);
                     })
                     .UseKestrel(opts =>
                     {
                         opts.Limits.MaxRequestBodySize = 524288000;
                     })
                     .UseDefaultServiceProvider((context, options) =>
                     {
                         options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                     })
                     .UseSetting(WebHostDefaults.HostingStartupAssembliesKey, BuildHostingStartupAssemblies())
                     .UseStartup<Startup>();
                });

        private static string BuildHostingStartupAssemblies()
        {
            return string.Join(";", new[]
            {
                "yumaster.FileService.Db",
                "yumaster.FileService.Service"
            });
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            logger.LogError($"{nameof(TaskScheduler_UnobservedTaskException)}: {e.Exception}");
        }
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        })
    }
}
