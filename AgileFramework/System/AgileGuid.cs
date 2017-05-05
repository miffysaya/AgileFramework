using System;

namespace AgileFramework
{
    /// <summary>
    /// Guid帮助类
    /// </summary>
    public static class AgileGuid
    {
        /// <summary>
        /// 获取大写GUID
        /// </summary>
        /// <returns>大写GUID</returns>
        public static string UpperNewGuid()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        /// <summary>
        /// 获取小写GUID
        /// </summary>
        /// <returns>小写GUID</returns>
        public static string LowerNewGuid()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }
    }
}
