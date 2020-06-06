using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using yumaster.FileService.Db.Repositories;
using yumaster.FileService.Db.Repositories.Impls;

[assembly:HostingStartup(typeof(yumaster.FileService.Db.DbHostingStartup))]
namespace yumaster.FileService.Db
{
    /// <summary>
    /// DB层服务配置
    /// </summary>
    public class DbHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<MasterDbContext>();

                services.AddScoped<IFileRepository, FileRepository>();
                services.AddScoped<IOwnerRepository, OwnerRepository>();

                services.AddSingleton<IRepositoryAccessor, RepositoryAccessor>();
            });
        }
    }
}
