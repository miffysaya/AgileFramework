using System.Web.Mvc;

namespace AgileFramework.Web.Mvc
{
    /// <summary>
    /// Text结果
    /// </summary>
    public class AgileTextResult : ContentResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AgileTextResult()
            : base()
        {
            ContentType = "text/plain";
        }
    }
}
