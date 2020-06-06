using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yumaster.FileService.WebApi.AutoReview
{
    public static class AppContextFactory
    {
        private static AppContext _instance;
        public static AppContext Instance => _instance ?? (_instance = new AppContext());
    }
}
