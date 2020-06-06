using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Service.Exceptions
{
    /// <summary>
    /// 友好提示异常
    /// </summary>
    public class FriendlyException : Exception
    {
        public FriendlyException()
        {
        }

        public FriendlyException(string message)
            : base(message)
        {
        }

        public FriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
