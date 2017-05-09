using System.Collections.Generic;
using System.IO;

namespace AgileFramework.IO
{
    /// <summary>
    /// 文件监控
    /// </summary>
    public static class AgileFileSystemWatcher
    {
        private static Dictionary<string, FileSystemWatcher> fileSystemWatcherList = new Dictionary<string, FileSystemWatcher>();
        /// <summary>
        /// 添加被监视的文件，一个文件只能被添加一次
        /// </summary>
        /// <param name="path">文件的路径，必须是全路径</param>
        /// <param name="changedEventHandler">发生变化时调用的方法</param>
        public static void Add(string path, FileSystemEventHandler changedEventHandler)
        {
            path = path.ToLower();
            if (!fileSystemWatcherList.ContainsKey(path))
            {
                FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(path), Path.GetFileName(path));
                fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                fileSystemWatcher.Changed += changedEventHandler;
                fileSystemWatcher.EnableRaisingEvents = true;
                fileSystemWatcherList.Add(path, fileSystemWatcher);
            }
        }
        /// <summary>
        /// 删除对指定文件的监视
        /// </summary>
        /// <param name="path">文件的路径，必须和添加时相同</param>
        public static void Remove(string path)
        {
            path = path.ToLower();
            if (fileSystemWatcherList.ContainsKey(path))
            {
                using (FileSystemWatcher fileSystemWatcher = fileSystemWatcherList[path])
                {
                    fileSystemWatcherList.Remove(path);
                    fileSystemWatcher.EnableRaisingEvents = false;
                }
            }
        }
        /// <summary>
        /// 删除全部文件的监视
        /// </summary>
        public static void Clear()
        {
            List<string> keys = new List<string>();
            keys.AddRange(fileSystemWatcherList.Keys);
            keys.ForEach(key => Remove(key));
        }
    }
}
