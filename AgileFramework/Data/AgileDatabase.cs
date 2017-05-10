using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.OracleClient;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;
using System.Transactions;

namespace AgileFramework.Data
{
    public static class AgileDatabase
    {
        /// <summary>
        /// 根据数据库类型获得某种数据库系列对象创建工厂
        /// </summary>
        /// <param name="databaseType">数据库类型</param>
        /// <returns>创建工厂</returns>
        public static DbProviderFactory GetDbProviderFactory(AgileDatabaseType databaseType = AgileDatabaseType.SqlServer)
        {
            DbProviderFactory instance = null;
            switch (databaseType)
            {
                case AgileDatabaseType.SqlServer:
                    instance = SqlClientFactory.Instance;
                    break;
                case AgileDatabaseType.OleDb:
                    instance = OleDbFactory.Instance;
                    break;
                case AgileDatabaseType.Oracle:
                    instance = OracleClientFactory.Instance;
                    break;
                case AgileDatabaseType.MySql:
                    instance = MySqlClientFactory.Instance;
                    break;
            }
            return instance;
        }

        /// <summary>
        /// 执行INSERT、UPDATE、DELETE以及不返回数据集的存储过程
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sentence">SQL命令或存储过程名</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="smartDatabaseType">数据库类型</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, string sentence, DbParameter[] parameters = null, AgileDatabaseType databaseType = AgileDatabaseType.SqlServer)
        {
            DbProviderFactory factory = GetDbProviderFactory(databaseType);
            using (DbConnection dbConnection = factory.CreateConnection())
            {
                dbConnection.ConnectionString = connectionString;
                using (DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = sentence;
                    //操作超时时间，目前设置为3分钟
                    dbCommand.CommandTimeout = 180;
                    if (parameters != null)
                    {
                        dbCommand.Parameters.AddRange(parameters);
                    }
                    dbConnection.Open();
                    return dbCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行SELECT以及返回数据集的存储过程，返回读取器。读取器关闭时，连接会自动关闭。不建议在BS项目中使用，因为传统的DataSet方式要更容易控制缓存。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sentence">SQL命令或存储过程名</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="databaseType">数据库类型</param>
        /// <returns>读取器</returns>
        public static DbDataReader ExecuteReader(string connectionString, string sentence, DbParameter[] parameters = null, AgileDatabaseType databaseType = AgileDatabaseType.SqlServer)
        {
            DbProviderFactory factory = GetDbProviderFactory(databaseType);
            DbConnection dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = connectionString;
            try
            {
                using (DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = sentence;
                    dbCommand.CommandTimeout = 0;
                    if (parameters != null)
                    {
                        dbCommand.Parameters.AddRange(parameters);
                    }
                    dbConnection.Open();
                    return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception exception)
            {
                //如果发生异常才关闭连接，然后抛出异常信息。如果未发生异常，不能关闭连接，连接是由客户端程序员控制。
                dbConnection.Close();
                throw exception;
            }
        }

        /// <summary>
        /// 返回数据集的第一行第一列。数据库中为Null或空，都返回Null
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sentence">SQL命令或存储过程名</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据集的第一行第一列</returns>
        public static object ExecuteScalar(string connectionString, string sentence, DbParameter[] parameters = null, AgileDatabaseType databaseType = AgileDatabaseType.SqlServer)
        {
            object result = null;
            DbProviderFactory factory = GetDbProviderFactory();
            using (DbConnection dbConnection = factory.CreateConnection())
            {
                dbConnection.ConnectionString = connectionString;
                using (DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = sentence;
                    dbCommand.CommandTimeout = 0;
                    if (parameters != null)
                    {
                        dbCommand.Parameters.AddRange(parameters);
                    }
                    dbConnection.Open();
                    result = dbCommand.ExecuteScalar();
                }
            }
            //如果返回的是DBNull类型或者是空，则返回null
            if (result == DBNull.Value || result.ToString().Trim().Length == 0)
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 填充数据集（数据集不是作为返回值，而是作为参数传入，这个决策主要是为了可以使用强类型的数据集。传入强类型的数据集也可以使用）
        /// </summary>
        /// <param name="dataSet">数据集（可以是强类型的数据集）</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sentence">SQL语句或存储过程名</param>
        /// <param name="parameters">参数数组</param>
        public static void Fill(string connectionString, string sentence, DataSet dataSet, DbParameter[] parameters = null, AgileDatabaseType databaseType = AgileDatabaseType.SqlServer)
        {
            DbProviderFactory factory = GetDbProviderFactory();
            DbConnection dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = connectionString;

            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sentence;
                dbCommand.CommandTimeout = 0;
                if (parameters != null)
                {
                    dbCommand.Parameters.AddRange(parameters);
                }
                using (DbDataAdapter dbDataAdapter = factory.CreateDataAdapter())
                {
                    dbDataAdapter.SelectCommand = dbCommand;
                    dbDataAdapter.Fill(dataSet);
                }
            }
        }

        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <param name="dataTable">数据表（可以是强类型的数据表）</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sentence">SQL语句或存储过程名</param>
        /// <param name="parameters">参数数组</param>
        public static void Fill(string connectionString, string sentence, DataTable dataTable, DbParameter[] parameters = null, AgileDatabaseType databaseType = AgileDatabaseType.SqlServer)
        {
            DbProviderFactory factory = GetDbProviderFactory();
            DbConnection dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = connectionString;

            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sentence;
                dbCommand.CommandTimeout = 0;
                if (parameters != null)
                {
                    dbCommand.Parameters.AddRange(parameters);
                }
                using (DbDataAdapter dbDataAdapter = factory.CreateDataAdapter())
                {
                    dbDataAdapter.SelectCommand = dbCommand;
                    dbDataAdapter.Fill(dataTable);
                }
            }
        }

        /// <summary>
        /// 返回安全字符串
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>返回</returns>
        public static string GetSafetySql(string input)
        {
            Regex keywordRegex = new Regex(@"\bexec\b|\binsert\b|\bdelete\b|\bupdate\b|\bdrop\b|\btruncate\b|\bselect\b|\bfrom\b|=|;|'|<>|!=|\balter\b|\bwhere\b|\bjoin\b", RegexOptions.IgnoreCase);

            return keywordRegex.Replace(input, "");
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="actions">要执行的操作</param>
        public static void TransactionScope(Action[] actions)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    actions[i]();
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="action">要执行的操作</param>
        public static void TransactionScope(Action action)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                action();

                scope.Complete();
            }
        }

        /// <summary>
        /// 按事务方式执行SmartSqlCommandLine命令列表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="smartSqlCommandLineList">命令列表</param>
        /// <param name="smartDatabaseType">数据库类型</param>
        public static void ExecuteWithTransaction(string connectionString, AgileSQLCommand sqlCommand, AgileDatabaseType databaseType = AgileDatabaseType.SqlServer)
        {
            DbProviderFactory factory = GetDbProviderFactory(databaseType);
            using (DbConnection dbConnection = factory.CreateConnection())
            {
                dbConnection.ConnectionString = connectionString;
                dbConnection.Open();
                using (DbTransaction dbTransaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.Transaction = dbTransaction;
                            dbCommand.CommandText = sqlCommand.SqlSentence;

                            //操作超时时间，目前设置为3分钟
                            dbCommand.CommandTimeout = 180;
                            if (sqlCommand.Parameters != null)
                            {
                                dbCommand.Parameters.AddRange(sqlCommand.Parameters);
                            }
                            //认为需要回滚的事务都是不需要返回结果的命令
                            dbCommand.ExecuteNonQuery();

                            if (sqlCommand.RollbackAction != null)
                            {
                                try
                                {
                                    sqlCommand.RollbackAction();
                                }
                                catch (Exception error)
                                {
                                    dbTransaction.Rollback();
                                    if (!string.IsNullOrWhiteSpace(sqlCommand.Error))
                                    {
                                        error = new Exception(sqlCommand.Error);
                                    }
                                    throw error;
                                }
                            }
                        }
                        dbTransaction.Commit();
                    }
                    catch (Exception err)
                    {
                        dbTransaction.Rollback();
                        throw err;
                    }
                }
            }
        }
    }
}
