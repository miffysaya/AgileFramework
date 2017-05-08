using System;
using System.Diagnostics;

namespace AgileFramework.Diagnostics
{
    /// <summary>
    /// 秒表
    /// </summary>
    public static class AgileStopwatch
    {
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="action">行为</param>
        /// <returns>结果</returns>
        public static TimeSpan Execute(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
