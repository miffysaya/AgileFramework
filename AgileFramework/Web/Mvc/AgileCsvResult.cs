using AgileFramework.Office;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AgileFramework.Web.Mvc
{
    public class AgileCsvResult : ContentResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rows">数据</param>
        /// <param name="fileName">文件名</param>
        public AgileCsvResult(List<dynamic> rows, string fileName = "Export")
            : base()
        {
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".csv");

            this.Content = AgileCsv.ToCsv(rows);

            this.ContentType = "text/csv";

            this.ContentEncoding = Encoding.Default;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rows">数据</param>
        /// <param name="fileName">文件名</param>
        public AgileCsvResult(DataTable rows, string fileName = "Export")
            : base()
        {
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".csv");

            this.Content = AgileCsv.ToCsv(rows);

            this.ContentType = "text/csv";

            this.ContentEncoding = Encoding.Default;
        }
    }
}
