using AgileFramework.Text;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AgileFramework.Net
{
    /// <summary>
    /// Socket通信
    /// </summary>
    public static class AgileSocket
    {
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="ip">端点IP</param>
        /// <param name="port">端点Port</param>
        /// <param name="message">消息</param>
        /// <param name="flgReceive">是否接受消息</param>
        /// <param name="encoding">数据编码方式</param>
        /// <returns>结果</returns>
        public static string Send(string ip, int port, string message, bool flgReceive = true, Encoding encoding = null)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            return Send(iPEndPoint, message, flgReceive, encoding);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="ipEndPoint">端点</param>
        /// <param name="message">消息</param>
        /// <param name="flgReceive">是否接受消息</param>
        /// <param name="encoding">数据编码方式</param>
        /// <returns>结果</returns>
        public static string Send(IPEndPoint ipEndPoint, string message, bool flgReceive = true, Encoding encoding = null)
        {
            encoding = encoding ?? AgileEncoding.Default;
            var result = string.Empty;
            using (Socket handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                handler.SendBufferSize = int.MaxValue;
                handler.SendTimeout = 0;
                handler.ReceiveBufferSize = int.MaxValue;
                handler.ReceiveTimeout = 100;

                handler.Connect(ipEndPoint);

                handler.Send(encoding.GetBytes(message));

                if (!flgReceive)
                {
                    return result;
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    while (true)
                    {
                        byte[] data = new byte[1024 * 100];

                        int dataLength = handler.Receive(data);
                        if (dataLength > 0)
                        {
                            memoryStream.Write(data, 0, data.Length);
                        }

                        if (dataLength == 0 || handler.Available == 0)
                        {
                            break;
                        }
                    }
                    memoryStream.Position = 0;

                    var array = memoryStream.ToArray();
                    result = encoding.GetString(array, 0, array.Length).Replace("\0", "");
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                    return result;
                }
            }
        }

        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="ipEndPoint">端点</param>
        /// <param name="func">动作</param>
        public static void Listen(IPEndPoint ipEndPoint, Func<string, Socket, string> func)
        {
            using (Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                listener.Bind(ipEndPoint);
                listener.Listen(100);

                while (true)
                {
                    try
                    {
                        using (Socket handler = listener.Accept())
                        {
                            handler.SendBufferSize = int.MaxValue;
                            handler.SendTimeout = 0;
                            handler.ReceiveBufferSize = int.MaxValue;
                            handler.ReceiveTimeout = 0;

                            string input = string.Empty;

                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                #region 获得请求
                                while (true)
                                {
                                    byte[] data = new byte[1024 * 100];
                                    int dataLength = handler.Receive(data);
                                    if (dataLength > 0)
                                    {
                                        memoryStream.Write(data, 0, data.Length);
                                    }

                                    if (dataLength < 1024 * 100)
                                    {
                                        break;
                                    }
                                }
                                var array = memoryStream.ToArray();
                                input = Encoding.UTF8.GetString(array, 0, array.Length);
                                input = input.Replace("\0", "");

                                var output = func(input, handler);

                                var index = 0;
                                while (index < output.Length)
                                {
                                    var current = string.Empty;

                                    if (index + 1024 * 100 > output.Length)
                                    {
                                        current = output.Substring(index, output.Length - index);
                                    }
                                    else
                                    {
                                        current = output.Substring(index, 1024 * 100);
                                    }

                                    handler.Send(Encoding.UTF8.GetBytes(current));

                                    index = index + 1024 * 100;
                                }
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                                #endregion
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// 获得远程IP
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>结果</returns>
        public static string GetRemoteEndPointIP(Socket socket)
        {
            SocketAddress socketAddress = socket.RemoteEndPoint.Serialize();

            return socketAddress[4].ToString() + "." + socketAddress[5].ToString() + "." + socketAddress[6].ToString() + "." + socketAddress[7].ToString();
        }

        /// <summary>
        /// 获得远程端口
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>结果</returns>
        public static int GetRemoteEndPointPort(Socket socket)
        {
            SocketAddress socketAddress = socket.RemoteEndPoint.Serialize();

            return socketAddress[2] * 256 + socketAddress[3];
        }
    }
}
