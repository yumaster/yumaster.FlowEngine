using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FileService.Db.Options
{
    public class DbOption
    {
        public DatabaseType DbType { get; set; }
        public string MasterConnectionString { get; set; }
        /// <summary>
        /// File表的分表个数
        /// </summary>
        public uint FileTableCount { get; set; }
    }

    public enum DatabaseType
    {
        MySql = 1,
        SqlServer = 2
    }
}
