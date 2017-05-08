using AgileFramework.Web;
using System;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace AgileFramework
{
    /// <summary>
    /// 异常信息帮助类
    /// </summary>
    public static class AgileException
    {
        /// <summary>
        /// 转换为Xml
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns>结果</returns>
        public static string ToXml(Exception exception)
        {
            var element = new XElement("Exception",
                                       new XElement("OccurTime") { Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") },
                                       new XElement("Message") { Value = HttpUtility.HtmlEncode(exception.Message) },
                                       new XElement("Source") { Value = HttpUtility.HtmlEncode(exception.Source) },
                                       new XElement("StackTrace") { Value = HttpUtility.HtmlEncode(exception.StackTrace) });

            return string.Format("<?xml version=\"1.0\" ?> {0}", element.ToString());
        }

        /// <summary>
        /// 转换为Dynamic
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns>结果</returns>
        public static string ToJson(Exception exception)
        {
            var obj = new
            {
                OccurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Message = HttpUtility.HtmlEncode(exception.Message),
                Source = HttpUtility.HtmlEncode(exception.Source),
                StackTrace = HttpUtility.HtmlEncode(exception.StackTrace)
            };

            return AgileJson.ToJson(obj);
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns>结果</returns>
        public static string ToString(Exception exception)
        {
            var messages = new StringBuilder();
            messages.AppendLine(exception.Message);
            messages.AppendLine(exception.Source);
            messages.Append(exception.StackTrace);

            var aggregateException = exception.InnerException as AggregateException;

            if (aggregateException != null)
            {
                foreach (var oneException in aggregateException.InnerExceptions)
                {
                    messages.AppendLine("");
                    messages.AppendLine(oneException.Message);
                    messages.AppendLine(oneException.Source);
                    messages.Append(oneException.StackTrace);
                }
            }
            else
            {
                if (exception.InnerException != null)
                {
                    messages.AppendLine("");
                    messages.AppendLine(exception.InnerException.Message);
                    messages.AppendLine(exception.InnerException.Source);
                    messages.Append(exception.InnerException.StackTrace);
                }
            }

            return messages.ToString();
        }
    }
}
