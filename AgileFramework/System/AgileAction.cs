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
        /// 尝试执行指定行为
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="maxTryCount">最大尝试次数</param>
        /// <param name="tryInterval">尝试间隔（单位毫秒）</param>
        public static void TryExecute(Action action, int maxTryCount = 1, int tryInterval = 1 * 1000)
        {
            bool isSuccessed = false;

            var tryCount = 0;

            var outException = default(Exception);

            while (tryCount < maxTryCount)
            {
                try
                {
                    action();

                    isSuccessed = true;

                    break;
                }
                catch (Exception exception)
                {
                    outException = exception;

                    tryCount++;

                    Thread.Sleep(tryInterval);
                }
            }
            if (!isSuccessed)
            {
                if (outException != null)
                {
                    throw outException;
                }
                else
                {
                    throw new Exception("获取错误信息失败！");
                }
            }
        }

        /// <summary>
        /// 循环执行指定行为(失败跳出)
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="maxExecuteCount">最大尝试次数</param>
        public static void CircularExecute(Action action, int maxExecuteCount = 1)
        {
            var tryCount = 0;

            while (tryCount < maxExecuteCount)
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        /// <summary>
        /// 强制循环执行指定行为(失败不跳出)
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="maxExecuteCount">最大尝试次数</param>
        public static void ForceCircularExecute(Action action, int maxExecuteCount = 1)
        {
            var trycCount = 0;

            var outException = default(Exception);

            var isFailed = false;

            while (tryCount < maxExecuteCount)
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    isFailed = true;

                    outException = exception;
                }
            }
            if (isFailed)
            {
                if (outException != null)
                {
                    throw outException;
                }
                throw new Exception("获取错误信息失败");
            }
        }

        /// <summary>
        /// 判断执行是否失败
        /// </summary>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public static bool IsInvokeFailed(Action action)
        {
            bool isFailed = false;

            try
            {
                action();
            }
            catch
            {
                isFailed = true;
            }
            return isFailed;
        }
    }
}
