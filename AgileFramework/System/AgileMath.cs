using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace AgileFramework
{
    /// <summary>
    /// 数学帮助类
    /// </summary>
    public static class AgileMath
    {
        /// <summary>
        /// 分析表达式用到的正则表达式
        /// </summary>
        private static Regex regex = new Regex(@"([\+\-\*])", RegexOptions.Compiled);

        /// <summary>
        /// 利用分析计算表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>结果</returns>
        public static double Eval(string expression)
        {
            var xpath = string.Format("number({0})", regex.Replace(expression, " ${1} ").Replace("/", " div ").Replace("%", " mod "));

            return (double)new XPathDocument(new StringReader("<r/>")).CreateNavigator().Evaluate(xpath);
        }
    }
}
