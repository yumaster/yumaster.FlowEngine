using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace yumaster.FileService.WebApi.AutoReview
{
    //
    // 摘要:
    //     APP上下文信息
    public class AppContext
    {
        private Assembly[] _referencedAssemblies;

        public IServiceCollection Services
        {
            get;
            internal set;
        }

        public IEnumerable<IAssert> Asserts
        {
            get;
            internal set;
        }

        //
        // 摘要:
        //     获取当前项目引用的所有程序集
        public Assembly[] ReferencedAssemblies
        {
            get
            {
                if (_referencedAssemblies == null)
                {
                    Dictionary<string, Assembly> dictionary = new Dictionary<string, Assembly>();
                    ScanReferencedAssemblies(Assembly.GetEntryAssembly(), dictionary);
                    _referencedAssemblies = Enumerable.ToArray(dictionary.Values);
                }
                return _referencedAssemblies;
            }
        }

        private void ScanReferencedAssemblies(Assembly asm, Dictionary<string, Assembly> asmDict)
        {
            if (!asmDict.ContainsKey(asm.FullName))
            {
                asmDict[asm.FullName] = asm;
                AssemblyName[] referencedAssemblies = asm.GetReferencedAssemblies();
                for (int i = 0; i < referencedAssemblies.Length; i++)
                {
                    Assembly asm2 = Assembly.Load(referencedAssemblies[i]);
                    ScanReferencedAssemblies(asm2, asmDict);
                }
            }
        }
    }
}
