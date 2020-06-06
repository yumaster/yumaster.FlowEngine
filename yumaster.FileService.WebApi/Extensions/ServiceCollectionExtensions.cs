using Microsoft.Extensions.DependencyInjection;
using yumaster.FileService.WebApi.AutoReview;

namespace yumaster.FileService.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AutoReview功能 此方法要放到所有注册服务代码后调用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="asserts"></param>
        public static void AddAutoReview(this IServiceCollection services, params IAssert[] asserts)
        {
            AppContext instance = AppContextFactory.Instance;
            instance.Services = services;
            instance.Asserts = asserts;
        }
    }
}
