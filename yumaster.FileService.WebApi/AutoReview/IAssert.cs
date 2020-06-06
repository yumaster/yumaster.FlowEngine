using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yumaster.FileService.WebApi.AutoReview
{
    public interface IAssert
    {
        void Assert(AppContext context);
    }
}
