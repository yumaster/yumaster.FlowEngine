﻿namespace yumaster.FileService.Service.Models.Result
{
    /// <summary>
    /// 带附加Data的操作结果接口定义
    /// </summary>
    public interface IDataResult<T> : IResult where T : class
    {
        T Data { get; set; }
    }
}
