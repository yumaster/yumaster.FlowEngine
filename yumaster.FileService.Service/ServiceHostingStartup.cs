using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using yumaster.FileService.Authorization;
using yumaster.FileService.Service.ServiceImpls;

[assembly: HostingStartup(typeof(yumaster.FileService.Service.ServiceHostingStartup))]
namespace yumaster.FileService.Service
{
    /// <summary>
    /// 服务层服务配置
    /// </summary>
    public class ServiceHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IJsonSerializer, DefaultJsonSerializer>();
                services.AddSingleton<ClusterService>();
                services.AddSingleton<IServerElectPolicy, WeightRoundServerElect>();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    services.AddSingleton<IImageConverter, MagickImageConverter>();
                else
                    services.AddSingleton<IImageConverter, CmdMagickImageConverter>();

                services.AddSingleton<IStorageService, DefaultStorageService>();
                services.AddSingleton<AppSecretSigner>();
                services.AddSingleton<FileUploadService>();

                services.AddHttpClient();
            });
        }
    }
}
