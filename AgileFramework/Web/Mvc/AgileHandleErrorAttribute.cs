using System.Web.Mvc;

namespace AgileFramework.Web.Mvc
{
    /// <summary>
    /// Mvc错误处理
    /// </summary>
    public class AgileHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 重写OnException
        /// </summary>
        /// <param name="filterContext">上下文</param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            filterContext.HttpContext.Response.StatusCode = 200;
        }
    }
}
