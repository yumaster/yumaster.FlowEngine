using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace yumaster.FileService.Authorization.Utils
{
    /// <summary>
    /// ProcessUtil
    /// </summary>
    public static class ProcessUtil
    {
        //
        // 摘要:
        //     静默启动一个新进程
        //
        // 参数:
        //   procPath:
        //
        //   args:
        //
        //   beforeStart:
        //     启动前需要执行的操作
        public static Process SilentStart(string procPath, string args, Action<ProcessStartInfo> beforeStart = null)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(procPath, args)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            beforeStart?.Invoke(processStartInfo);
            Process process = Process.Start(processStartInfo);
            process.StandardInput.AutoFlush = true;
            return process;
        }

        //
        // 摘要:
        //     静默执行一个命令并返回结果，返回int.MinValue表示进程超时
        //
        // 参数:
        //   procPath:
        //
        //   args:
        //
        //   standardOutput:
        //
        //   standardError:
        //
        //   timeoutMillisecond:
        //     超时值，-1表示无期限
        //
        // 返回结果:
        //     返回进程退出代码
        public static int ExecuteCommand(string procPath, string args, out string standardOutput, out string standardError, int timeoutMillisecond = -1)
        {
            Process process = SilentStart(procPath, args);
            try
            {
                string strStandOut = "";
                string strErrOut = "";
                process.OutputDataReceived += delegate (object s1, DataReceivedEventArgs e1)
                {
                    if (e1.Data != null)
                    {
                        strStandOut = strStandOut + e1.Data + "\r\n";
                    }
                };
                process.ErrorDataReceived += delegate (object s1, DataReceivedEventArgs e1)
                {
                    if (e1.Data != null)
                    {
                        strErrOut = strErrOut + e1.Data + "\r\n";
                    }
                };
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                if (timeoutMillisecond >= 0)
                {
                    process.WaitForExit(timeoutMillisecond);
                }
                else
                {
                    process.WaitForExit();
                }
                standardOutput = strStandOut;
                standardError = strErrOut;
                if (process.HasExited)
                {
                    return process.ExitCode;
                }
                try
                {
                    process.Kill();
                }
                catch
                {
                }
                return int.MinValue;
            }
            finally
            {
                ((IDisposable)process)?.Dispose();
            }
        }
    }
}
