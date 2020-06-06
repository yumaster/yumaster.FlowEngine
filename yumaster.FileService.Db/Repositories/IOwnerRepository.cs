using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace yumaster.FileService.Db.Repositories
{
    /// <summary>
    /// 文件所有者相关的数据访问接口
    /// </summary>
    public interface IOwnerRepository : IRepository
    {
        /// <summary>
        /// 获取指定用户剩余的配额
        /// </summary>
        Task<long> GetOwnerRemainQuotaAsync(int ownerType, long ownerId);
        /// <summary>
        /// 为指定的用户增加已使用的配额数
        /// </summary>
        Task AddOwnerUsedQuotaAsync(int ownerType, long ownerId, long fileLength);
        /// <summary>
        /// 为指定的用户减少已使用的配额数
        /// </summary>
        Task DecreaseOwnerUsedQuotaAsync(int ownerType, long ownerId, long fileLength);
        /// <summary>
        /// 设置指定用户的最大配额（不存在则创建）
        /// </summary>
        Task SetOwnerMaxQuotaAsync(int ownerType, long ownerId, long max);
    }
}
