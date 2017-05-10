using System.Data;
using System.Data.SqlClient;

namespace AgileFramework.Data
{
    /// <summary>
    /// SQLServer特有功能
    /// </summary>
    public static class AgileSQLServer
    {
        /// <summary>
        /// 批量拷贝
        /// </summary>
        /// <param name="sqlConnectionString">数据库连接字符串</param>
        /// <param name="sourceDataTable">数据源（必须与目标表结构完全一致）</param>
        /// <param name="targetTableName">目标表名</param>
        public static void BulkCopy(string sqlConnectionString, DataTable sourceDataTable, string targetTableName)
        {
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnectionString))
            {
                sqlBulkCopy.DestinationTableName = targetTableName;
                sqlBulkCopy.WriteToServer(sourceDataTable);
            }
        }
    }
}
