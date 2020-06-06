using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace yumaster.FileService.WebApi.AutoReview
{
    //
    // 摘要:
    //     依赖注入断言
    public class DependencyInjectionAssert : IAssert
    {
        //
        // 摘要:
        //     要忽略的类型FullName
        public string[] IgnoreTypes
        {
            get;
            set;
        }

        public void Assert(AppContext appContext)
        {
            string[] array = new string[2]
            {
                "Microsoft.Extensions.DependencyInjection.TelemetryConfigurationOptionsSetup",
                "Microsoft.AspNetCore.Hosting.Internal.HostedServiceExecutor"
            };
            string[] source = array;
            if (IgnoreTypes != null)
            {
                source = Enumerable.ToArray(Enumerable.Concat(array, IgnoreTypes));
            }
            StringBuilder stringBuilder = new StringBuilder();
            IServiceCollection services = appContext.Services;
            foreach (ServiceDescriptor item in services)
            {
                Type implementationType = item.ImplementationType;
                if ((object)implementationType != null && !Enumerable.Contains(source, implementationType.FullName))
                {
                    List<ConstructorInfo> list = Enumerable.ToList(Enumerable.OrderByDescending(TypeExtensions.GetConstructors(implementationType), (ConstructorInfo p) => p.GetParameters().Length));
                    List<ConstructorInfo> list2 = new List<ConstructorInfo>();
                    int num = -1;
                    foreach (ConstructorInfo item2 in list)
                    {
                        List<Type> list3 = Enumerable.ToList(Enumerable.Select(item2.GetParameters(), (ParameterInfo p) => p.ParameterType));
                        if (!Enumerable.Any(GetServiceDescriptorByTypes(services, list3), (ServiceDescriptor p) => p == null) && (num == -1 || list3.Count == num))
                        {
                            num = list3.Count;
                            list2.Add(item2);
                        }
                    }
                    if (list2.Count == 0)
                    {
                        stringBuilder.AppendLine($"Service {implementationType.FullName} has no suitable construction");
                    }
                    else if (list2.Count > 1)
                    {
                        string arg = string.Join("\r\n", list2);
                        stringBuilder.AppendLine($"The type {implementationType} following constructors are ambigious:\r\n{arg}");
                    }
                    else
                    {
                        List<Type> list4 = Enumerable.ToList(Enumerable.Select(Enumerable.First(list2).GetParameters(), (ParameterInfo p) => p.ParameterType));
                        IList<ServiceDescriptor> serviceDescriptorByTypes = GetServiceDescriptorByTypes(services, list4);
                        for (int i = 0; i < list4.Count; i++)
                        {
                            ServiceDescriptor serviceDescriptor = serviceDescriptorByTypes[i];
                            Type type = list4[i];
                            if (item.Lifetime == ServiceLifetime.Singleton && serviceDescriptor.Lifetime == ServiceLifetime.Scoped)
                            {
                                stringBuilder.AppendLine($"The singleton service {implementationType.FullName} constructor references scoped service {type.FullName}");
                            }
                        }
                    }
                }
            }
            if (stringBuilder.Length > 0)
            {
                throw new InvalidProgramException(stringBuilder.ToString());
            }
        }

        private IList<ServiceDescriptor> GetServiceDescriptorByTypes(IServiceCollection services, ICollection<Type> types)
        {
            List<ServiceDescriptor> list = new List<ServiceDescriptor>();
            foreach (Type type2 in types)
            {
                if ((object)type2 == typeof(IServiceProvider))
                {
                    list.Add(new ServiceDescriptor(typeof(IServiceProvider), new object()));
                }
                else
                {
                    TypeInfo tInfo = type2.GetTypeInfo();
                    if (tInfo.IsGenericType && type2.ToString().StartsWith("System.Collections.Generic.IEnumerable`1"))
                    {
                        Type type = Enumerable.First(tInfo.GetGenericArguments());
                        tInfo = type.GetTypeInfo();
                    }
                    ServiceDescriptor item;
                    if (tInfo.IsGenericType)
                    {
                        string prefix = Regex.Match(tInfo.ToString(), "^[^`]+`\\d+\\[").Value;
                        item = Enumerable.FirstOrDefault(services, (ServiceDescriptor p) => p.ServiceType.ToString().StartsWith(prefix));
                    }
                    else
                    {
                        item = Enumerable.FirstOrDefault(services, (ServiceDescriptor p) => (object)p.ServiceType == tInfo.AsType());
                    }
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
