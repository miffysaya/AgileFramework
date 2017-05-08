using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace AgileFramework.Office
{
    /// <summary>
    /// Csv文件处理帮助类
    /// </summary>
    public static class AgileCsv
    {
        /// <summary>
        /// 转换为Csv
        /// </summary>
        /// <param name="rows">数据源列表</param>
        /// <returns>结果</returns>
        public static string ToCsv(List<dynamic> rows)
        {
            var result = new StringBuilder();

            if (rows == null || rows.Count == 0)
            {
                return string.Empty;
            }

            var properties = rows.First().GetType().GetProperties() as PropertyInfo[];

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];

                if (i == properties.Length - 1)
                {
                    result.AppendLine("\"" + property.Name + "\"");
                }
                else
                {
                    result.Append("\"" + property.Name + "\"" + ",");
                }
            }

            var j = 0;

            rows.ForEach(item =>
            {
                j++;
                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];

                    if (i == properties.Length - 1)
                    {
                        if (j == rows.Count)
                        {
                            result.Append("\"" + property.GetValue(item, null).ToString().Replace("\"", "\"\"") + "\"");
                        }
                        else
                        {
                            result.AppendLine("\"" + property.GetValue(item, null).ToString().Replace("\"", "\"\"") + "\"");
                        }
                    }
                    else
                    {
                        result.Append("\"" + property.GetValue(item, null).ToString().Replace("\"", "\"\"") + "\"" + ",");
                    }
                }
            });

            return result.ToString();
        }

        /// <summary>
        /// 转换为Csv
        /// </summary>
        /// <param name="rows">数据源表格</param>
        /// <returns>结果</returns>
        public static string ToCsv(DataTable rows)
        {
            var result = new StringBuilder();

            int columnCount = rows.Columns.Count;

            int rowCount = rows.Rows.Count;

            if (rows == null || rows.Rows.Count == 0)
            {
                return string.Empty;
            }

            for (int i = 0; i < columnCount; i++)
            {
                var columnName = rows.Columns[i].ColumnName;

                if (i == columnCount - 1)
                {
                    result.AppendLine("\"" + columnName + "\"");
                }
                else
                {
                    result.Append("\"" + columnName + "\"" + ",");
                }
            }

            for (int j = 0; j < rowCount; j++)
            {
                var row = rows.Rows[j];

                for (int i = 0; i < columnCount; i++)
                {

                    if (i == columnCount - 1)
                    {
                        if (j == rowCount - 1)
                        {
                            result.Append("\"" + row[i].ToString().Replace("\"", "\"\"") + "\"");
                        }
                        else
                        {
                            result.AppendLine("\"" + row[i].ToString().Replace("\"", "\"\"") + "\"");
                        }
                    }
                    else
                    {
                        result.Append("\"" + row[i].ToString().Replace("\"", "\"\"") + "\"" + ",");
                    }
                }
            }

            return result.ToString();
        }
    }
}
