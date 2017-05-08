using AgileFramework.Text;
using System;

namespace AgileFramework
{
    /// <summary>
    /// 多用于服务端的接口调用验证
    /// </summary>
    public static class AgileToken
    {
        private static string skey = "76d027cb";

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string GetToken(string userHostAddress = "Web.AgileRequest.UserHostAddress", string userAgent = "HttpContext.Current.Request.UserAgent")
        {
            string content = string.Format
                (
                    "{0}^{1}^{2}^{3}",
                    AgileGuid.LowerNewGuid(),
                    DateTime.Now.ToString(),
                    userHostAddress,
                    userAgent
                );
            return AgileHex.ToHex(Security.AgileDES.Encrypt(content, skey), System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 校验token
        /// </summary>
        /// <param name="token">要校验的token</param>
        /// <param name="timeout">失效期限（秒）</param>
        /// <returns></returns>
        public static bool IsVerifyToken(string token, int timeout, string userHostAddress = "Web.AgileRequest.UserHostAddress", string userAgent = "HttpContext.Current.Request.UserAgent")
        {
            token = AgileHex.FromHex(token, System.Text.Encoding.UTF8);
            string[] values = Security.AgileDES.Decrypt(token, skey).Split('^');
            if (Convert.ToDateTime(values[1]).AddSeconds(timeout) < DateTime.Now
                || values[2] != userHostAddress
                || values[3] != userAgent)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
