using System;
using System.Data.Common;

namespace AgileFramework
{
    /// <summary>
    /// SQL语句包装
    /// </summary>
    public class AgileSQLCommand
    {
        /// <summary>
        /// 语句
        /// </summary>
        public string SqlSentence { get; }

        /// <summary>
        /// 报错信息
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// 参数
        /// </summary>
        public DbParameter[] Parameters { get; }

        /// <summary>
        /// 需要参与事务的程序动作
        /// </summary>
        public Action RollbackAction { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="parameters"></param>
        /// <param name="action"></param>
        /// <param name="error"></param>
        public AgileSQLCommand(string sentence, DbParameter[] parameters = null, Action action = null, string error = null)
        {
            SqlSentence = sentence;
            Error = error;
            Parameters = parameters;
            RollbackAction = action;
        }
    }
}
