using System;

namespace AgileFramework.Web
{
    /// <summary>
    /// Http上下文
    /// </summary>
    public static class AgileHttpContext
    {
        /// <summary>
        /// 启动时间
        /// </summary>
        public readonly static DateTime StartupTime = DateTime.Now;

        /// <summary>
        /// Bin目录（有斜线结尾）
        /// </summary>
        public static readonly string BinDirectory = Path.GetDirectoryName(HttpRuntime.BinDirectory).TrimEnd('\\') + "\\";

        /// <summary>
        /// 项目所在目录（有斜线结尾）
        /// </summary>
        public static readonly string Directory = Path.GetDirectoryName(HttpRuntime.AppDomainAppPath).TrimEnd('\\') + "\\";
    }
}
