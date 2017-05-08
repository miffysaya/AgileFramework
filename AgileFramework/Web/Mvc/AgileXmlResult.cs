using System.Web.Mvc;

namespace AgileFramework.Web.Mvc
{
    public class AgileXmlResult : ContentResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AgileXmlResult()
            : base()
        {
            this.ContentType = "text/xml";
        }
    }
}
