using System.Web.Mvc;

namespace AgileFramework.Web.Mvc
{
    /// <summary>
    /// Json结果
    /// </summary>
    public class AgileJsonResult : ContentResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AgileJsonResult()
            : base()
        {
            this.ContentType = "application/json";
        }
    }
}
