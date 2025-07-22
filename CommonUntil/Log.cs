// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUntil
{
    public static class Log
    {
        // 定义一个静态的锁对象，确保整个应用内共用这把锁
        private static object _logLock = new object();
        public static void WriteLog(string logContent,string logFileName = "Logs")
        {
            try
            {
                // 获取当前日期，格式为yyyy-MM-dd
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                // 构造日志文件路径，假设日志文件存放在项目目录下的Logs文件夹中
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName, $"{date}.txt");
                // 判断Logs文件夹是否存在，不存在则创建
                if (!Directory.Exists(Path.GetDirectoryName(logFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
                }
                // 追加写入日志内容，包含时间和日志信息
                string logEntry = $"{DateTime.Now}: {logContent}{Environment.NewLine}";
                //File.AppendAllText(logFilePath, logEntry);
                // 加锁，确保同一时间只有一个线程/任务能执行写入操作
                lock (_logLock)
                {
                    try
                    {
                        File.AppendAllText(logFilePath, logEntry);
                    }
                    catch (Exception ex)
                    {
                        // 这里可以做更完善的错误处理，比如记录到其他地方
                        Console.WriteLine($"写入日志时发生错误: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"写入日志时发生错误: {ex.Message}");
            }
        }
    }
}
