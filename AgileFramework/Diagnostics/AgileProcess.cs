using System.Collections.Generic;
using System.Diagnostics;

namespace AgileFramework.Diagnostics
{
    /// <summary>
    /// 提供进程相关的操作
    /// </summary>
    public static class AgileProcess
    {
        /// <summary>
        /// 启动程序，注意，默认的情况下，窗口正常打开，无参数
        /// </summary>
        /// <param name="processPath">待启动的程序路径</param>
        public static void Start(string processPath)
        {
            Start(processPath, ProcessWindowStyle.Normal, string.Empty);
        }
        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="processPath">待启动的程序路径</param>
        /// <param name="processWindowStyle">窗口样式</param>
        /// <param name="args">参数</param>
        public static void Start(string processPath, ProcessWindowStyle processWindowStyle, string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = processPath;
                process.StartInfo.Arguments = args;
                process.StartInfo.WindowStyle = processWindowStyle;
                process.Start();
            }
        }
        /// <summary>
        /// 执行命令行命令
        /// </summary>
        /// <param name="commandLine">命令，例如：ping 211.66.4.102</param>
        /// <returns>返回执行的结果</returns>
        public static string ExecuteCommandLine(string commandLine)
        {
            string output = string.Empty;
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine(commandLine);
                process.StandardInput.WriteLine("exit");
                output = process.StandardOutput.ReadToEnd();
            }
            return output;
        }
        /// <summary>
        /// 系统进程列表
        /// </summary>
        public static List<Process> AllProcesses
        {
            get
            {
                return new List<Process>(Process.GetProcesses());
            }
        }
        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="processName">进程名称</param>
        public static void Stop(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                process.EnableRaisingEvents = false;
                if (process.HasExited == false)
                {
                    //先尝试关闭主窗口，如果没有主窗口，就关闭主进程（关闭主窗口时会自动关闭主进程，反之不然，但是没有窗口的程序也只能通过关闭主进程关闭了）
                    if (process.CloseMainWindow() == false)
                    {
                        process.Kill();
                    }
                }
            }
        }
    }
}
