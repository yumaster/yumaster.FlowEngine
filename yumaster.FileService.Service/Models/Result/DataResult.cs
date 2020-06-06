using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Service.Models.Result
{
    public class DataResult<T> : Result, IDataResult<T> where T : class
    {
        public DataResult()
        {
        }

        public DataResult(int errorCode = ResultErrorCodes.Unknown, string errorMsg = null)
            : base(errorCode, errorMsg)
        {
        }

        /// <summary>
        /// API结果的数据内容
        /// </summary>
        public T Data { get; set; }
    }
}
