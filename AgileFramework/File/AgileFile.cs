using AgileFramework.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AgileFramework.IO
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public static class AgileFile
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="filePath">文件路径，文件不存在时会自动创建，存在时则追加信息</param>
        /// <param name="message">写入内容</param>
        /// <param name="isNewline">是否自动换行</param>
        /// <param name="append">是否在当前文本后附加,替换</param>
        /// <param name="encoding">编码方式</param>
        public static void Write(string filePath, string message, bool isNewline = true, Encoding encoding = null, bool append = true)
        {
            encoding = encoding ?? AgileEncoding.Default;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            catch { }

            using (var streamWriter = new StreamWriter(filePath, append, encoding))
            {
                if (isNewline)
                {
                    streamWriter.WriteLine(message);
                }
                else
                {
                    streamWriter.Write(message);
                }
                streamWriter.Flush();
            }
        }

        /// <summary>
        /// 获得读取流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="fileEncoding">文件编码</param>
        /// <returns>结果</returns>
        public static StreamReader GetStreamReader(string path, AgileFileType fileType = AgileFileType.Text, Encoding fileEncoding = null)
        {
            if (fileEncoding == null)
            {
                fileEncoding = Encoding.Default;
            }
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            if (fileType == AgileFileType.GZip)
            {
                GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);

                return new StreamReader(gzipStream, fileEncoding);
            }
            else
            {
                return new StreamReader(fileStream, fileEncoding);
            }
        }

        /// <summary>
        /// 获得输出流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="fileEncoding">文件编码</param>
        /// <param name="fileMode">文件模式</param>
        /// <param name="fileAccess">文件控制</param>
        /// <returns>结果</returns>
        public static StreamWriter GetStreamWriter(string path, AgileFileType fileType = AgileFileType.Text, Encoding fileEncoding = null, FileMode fileMode = FileMode.Append, FileAccess fileAccess = FileAccess.Write)
        {
            if (fileEncoding == null)
            {
                fileEncoding = Encoding.Default;
            }
            FileStream fileStream = new FileStream(path, fileMode, fileAccess);

            if (fileType == AgileFileType.GZip)
            {
                GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Compress);

                return new StreamWriter(gzipStream, fileEncoding);
            }
            else
            {
                return new StreamWriter(fileStream, fileEncoding);
            }
        }

        /// <summary>
        /// 将对象转成二进制文件存储到对应的目录
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="path">文件地址</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="fileMode">打开文件方式</param>
        /// <param name="fileAccess">文件访问方式</param>
        public static void BinaryWrite(object obj, string path, AgileFileType fileType = AgileFileType.Text, FileMode fileMode = FileMode.OpenOrCreate, FileAccess fileAccess = FileAccess.Write)
        {
            using (FileStream fileStream = new FileStream(path, fileMode, fileAccess))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                if (fileType == AgileFileType.Text)
                {
                    formatter.Serialize(fileStream, obj);
                }
                else
                {
                    using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
                    {
                        formatter.Serialize(gzipStream, obj);
                    }
                }

            }
        }

        /// <summary>
        /// 将二进制文件转成对象并返回
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="path">文件地址</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="fileMode">打开文件方式</param>
        /// <param name="fileAccess">文件访问方式</param>
        /// <returns>对象</returns>
        public static T BinaryRead<T>(string path, AgileFileType fileType = AgileFileType.Text, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream(path, fileMode, fileAccess))
            {
                if (fileType == AgileFileType.Text)
                {
                    return (T)formatter.Deserialize(fileStream);
                }
                else
                {
                    using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                    {
                        return (T)formatter.Deserialize(gzipStream);
                    }
                }

            }
        }
    }

    public enum AgileFileType
    {
        /// <summary>
        /// 文本文件
        /// </summary>
        Text = 1,

        /// <summary>
        /// 压缩文件
        /// </summary>
        GZip = 2
    }
}
