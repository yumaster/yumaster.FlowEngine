using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Db.Entities
{
    /// <summary>
    /// A shortcut of <see cref="IdEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public abstract class IdEntity : IdEntity<int>, IIdEntity
    {
    }

    /// <summary>
    /// IEntity接口的基本实现。
    /// 实体可以继承此类直接实现到IEntity接口。
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体的主键类型</typeparam>
    public abstract class IdEntity<TPrimaryKey> : IIdEntity<TPrimaryKey> where TPrimaryKey : struct
    {
        /// <summary>
        /// 此实体的唯一标识符
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is IdEntity<TPrimaryKey>))
            {
                return false;
            }

            //同样的例子必须被认为是相等的
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(IdEntity<TPrimaryKey> left, IdEntity<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(IdEntity<TPrimaryKey> left, IdEntity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}
