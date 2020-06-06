using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Db.Entities
{
    /// <summary>
    /// 定义基本实体类型的接口。系统中的所有实体都必须实现此接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体的主键类型</typeparam>
    public interface IIdEntity<TPrimaryKey> : IEntity where TPrimaryKey : struct
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }

    /// <summary>
    /// <see cref="IIdEntity{TPrimaryKey}"/> 最常用主键类型的快捷方式 (<see cref="int"/>).
    /// </summary>
    public interface IIdEntity : IIdEntity<int>
    {
    }
}
