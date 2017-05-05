using System;
using System.Threading;

namespace AgileFramework
{
    /// <summary>
    /// Action类
    /// </summary>
    public static class AgileAction
    {
        /// <summary>
        /// 尝试运行指定行为
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="maxTryCount">最大尝试次数</param>
        /// <param name="tryInterval">尝试间隔（单位毫秒）</param>
        public static void Try(Action action, int maxTryCount = 1, int tryInterval = 1 * 1000)
        {
            bool isSuccess = false;

            var tryCount = 0;

            var outException = default(Exception);

            while (tryCount < maxTryCount)
            {
                try
                {
                    action();

                    isSuccess = true;

                    break;
                }
                catch (Exception exception)
                {
                    outException = exception;

                    tryCount++;

                    Thread.Sleep(tryInterval);
                }
            }
            if (!isSuccess)
            {
                if (outException != null)
                {
                    throw outException;
                }
                else
                {
                    throw new Exception("尝试失败！");
                }
            }
        }
    }
}
