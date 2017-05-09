using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AgileFramework.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class AgileGZip
    {
        /// <summary>
        /// 缓存的大小
        /// </summary>
        private static int BUFFER_SIZE = 10000;
        /// <summary>
        /// 压缩流
        /// </summary>
        /// <param name="input">需要被压缩的流</param>
        /// <returns>压缩之后的流</returns>
        public static Stream Compress(Stream input)
        {
            MemoryStream output = new MemoryStream();
            using (GZipStream gzipStream = new GZipStream(output, CompressionMode.Compress, true))
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                int count = input.Read(buffer, 0, BUFFER_SIZE);
                while (count > 0)
                {
                    gzipStream.Write(buffer, 0, count);
                    count = input.Read(buffer, 0, BUFFER_SIZE);
                }
                gzipStream.Flush();
            }
            output.Position = 0;
            return output;
        }

        /// <summary>
        /// 解压流
        /// </summary>
        /// <param name="input">需要被解压的流</param>
        /// <returns>解压之后的流</returns>
        public static Stream Decompress(Stream input)
        {
            MemoryStream output = new MemoryStream();
            using (GZipStream gzipStream = new GZipStream(input, CompressionMode.Decompress, true))
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                int count = gzipStream.Read(buffer, 0, BUFFER_SIZE);
                while (count > 0)
                {
                    output.Write(buffer, 0, count);
                    count = gzipStream.Read(buffer, 0, BUFFER_SIZE);
                }
            }
            output.Position = 0;
            return output;
        }

        /// <summary>
        /// 压缩文件：被压缩的文件必须存在，压缩之后保存的文件路径必须不存在
        /// </summary>
        /// <param name="sourceFilePath">需要被压缩的文件</param>
        /// <param name="targetFilePath">压缩之后保存的文件路径</param>
        public static void Compress(string sourceFilePath, string targetFilePath)
        {
            using (Stream input = File.Open(sourceFilePath, FileMode.Open), output = Compress(input), fileStream = File.Create(targetFilePath))
            {
                byte[] data = new byte[output.Length];
                output.Read(data, 0, data.Length);
                fileStream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 解压缩文件：被解压缩的文件必须存在，解压之后保存的文件路径必须不存在
        /// </summary>
        /// <param name="sourceFilePath">需要被解压的文件</param>
        /// <param name="targetFilePath">解压之后保存的文件路径</param>
        public static void Decompress(string sourceFilePath, string targetFilePath)
        {
            using (Stream input = File.Open(sourceFilePath, FileMode.Open), output = Decompress(input), fileStream = File.Create(targetFilePath))
            {
                byte[] data = new byte[output.Length];
                output.Read(data, 0, data.Length);
                fileStream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static string Compress(string input)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(input))
            {
                byte[] source = Encoding.UTF8.GetBytes(input);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (GZipStream gzs = new GZipStream(memoryStream, CompressionMode.Compress, true))
                    {
                        gzs.Write(source, 0, source.Length);
                    }

                    memoryStream.Position = 0;

                    byte[] target = new byte[memoryStream.Length];
                    memoryStream.Read(target, 0, target.Length);

                    byte[] finalBuffer = new byte[target.Length + 4];
                    Buffer.BlockCopy(target, 0, finalBuffer, 4, target.Length);
                    Buffer.BlockCopy(BitConverter.GetBytes(source.Length), 0, finalBuffer, 0, 4);

                    result = System.Convert.ToBase64String(finalBuffer);
                }
            }

            return result;
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static string Decompress(string input)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(input))
            {
                byte[] source = System.Convert.FromBase64String(input);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int length = BitConverter.ToInt32(source, 0);
                    memoryStream.Write(source, 4, source.Length - 4);
                    memoryStream.Position = 0;
                    byte[] decmpBytes = new byte[length];
                    using (GZipStream gzs = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        gzs.Read(decmpBytes, 0, length);
                    }

                    result = Encoding.UTF8.GetString(decmpBytes);
                }
            }

            return result;
        }

        /// <summary>
        /// byte数组GZip压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                GZipStream Compress = new GZipStream(ms, CompressionMode.Compress);
                Compress.Write(bytes, 0, bytes.Length);
                Compress.Close();
                return ms.ToArray();

            }
        }

        /// <summary>
        /// byte数组GZip解压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] bytes)
        {
            using (MemoryStream tempMs = new MemoryStream())
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    GZipStream Decompress = new GZipStream(ms, CompressionMode.Decompress);
                    Decompress.CopyTo(tempMs);
                    Decompress.Close();
                    return tempMs.ToArray();
                }
            }
        }
    }
}
