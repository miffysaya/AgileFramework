using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;

namespace AgileFramework.Web
{
    /// <summary>
    /// 请求帮助类
    /// </summary>
    public static class AgileRequest
    {
        /// <summary>
        /// 用户IP
        /// </summary>
        public static string UserHostAddress
        {
            get
            {
                var request = HttpContext.Current.Request;

                var ip = "";

                if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Trim();
                }
                if (ip.Length == 0)
                {
                    ip = request.ServerVariables["REMOTE_ADDR"];
                }
                if (ip.IndexOf(",") > -1)
                {
                    string[] array = ip.Split(new char[] { ',' });
                    ip = array[array.Length - 1].Trim();
                }
                try
                {
                    IPAddress.Parse(ip);
                }
                catch
                {
                    ip = "0.0.0.0";
                }
                return ip;
            }
        }

        /// <summary>
        /// 主机端口号
        /// </summary>
        public static string UserHostPort
        {
            get
            {
                var request = HttpContext.Current.Request;

                string port = request.ServerVariables["HTTP_REMOTE_PORT"] ?? request.ServerVariables["REMOTE_PORT"];

                return string.IsNullOrEmpty(port) ? string.Empty : port;
            }
        }

        /// <summary>
        /// 获取URL内容
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="httpActionType">传参方式</param>
        /// <param name="postParameter">post参数</param>
        /// <returns></returns>
        public static string GetHtml(string url, Encoding encoding = null, int timeout = 1000, AgileHttpActionType httpActionType = AgileHttpActionType.Get, string postParameter = "")
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            var request = HttpWebRequest.Create(url);

            request.Proxy = null;

            request.Timeout = timeout;

            request.Headers.Add("Accept-Encoding", "gzip,deflate");

            if (httpActionType == AgileHttpActionType.Post)
            {
                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";

                if (!string.IsNullOrEmpty(postParameter))
                {
                    var postData = encoding.GetBytes(postParameter);

                    request.ContentLength = postData.Length;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(postData, 0, postData.Length);
                    }
                }
            }
            else
            {
                request.Method = "GET";
            }
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (var streamReader = new StreamReader(stream, encoding))
                        {
                            var html = streamReader.ReadToEnd();

                            return html;
                        }
                    }
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (var streamReader = new StreamReader(stream, encoding))
                        {
                            var html = streamReader.ReadToEnd();

                            return html;
                        }
                    }
                }
                else
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(responseStream, encoding))
                        {
                            var html = streamReader.ReadToEnd();

                            return html;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Http请求方式
    /// </summary>
    public enum AgileHttpActionType
    {
        /// <summary>
        /// Get传参
        /// </summary>
        Get,
        /// <summary>
        /// Post传参
        /// </summary>
        Post
    }
}

