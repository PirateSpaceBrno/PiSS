using System;
using System.IO;

namespace PiSS
{
    public class Logger
    {
        private static String _logFilePath = Environment.SpecialFolder.DesktopDirectory.ToString() + "\\PiSS.log";
        private FileInfo _logFile = new FileInfo(_logFilePath);
        private int logSize = 20 * 1024 * 1024;
        private bool IsDebugRun;

        public Logger(bool debugRun)
        {
            IsDebugRun = debugRun;
        }

        public enum logLevel
        {
            INFO,
            DEBUG,
            FAIL,
            FATAL
        }


        public void Log(string logMessage, logLevel logLevel, Exception exception = null)
        {
            string logLine = null;

            if ((logMessage == Environment.NewLine))
            {
                logLine = logMessage;
            }
            else
            {
                if ((logLevel == logLevel.DEBUG && IsDebugRun == false))
                {
                    return;
                }

                //Parse log text
                logLine = DateTime.Now.ToString("<yyyy-MM-dd HH:mm:ss>") + " [" + logLevel.ToString() + "] " + logMessage;

                if ((exception != null))
                {
                    logLine = logLine + " | Exception: " + exception.Message;

                    if ((IsDebugRun))
                    {
                        logLine = logLine + " | StackTrace: " + exception.StackTrace;
                    }

                    if ((exception.InnerException != null))
                    {
                        logLine = logLine + " | Exception: " + exception.InnerException.Message;

                        if ((IsDebugRun))
                        {
                            logLine = logLine + " | StackTrace: " + exception.InnerException.StackTrace;
                        }
                    }
                }
            }

            Create();
            Rollup();
            File.AppendText(_logFilePath);
        }


        private void Create()
        {
            if (_logFile.Exists == false)
            {
                File.Create(_logFilePath);
            }
        }

        private void Rollup()
        {
            if ((_logFile.Length >= logSize))
            {
                File.Move(_logFilePath, _logFile.Name + DateTime.Now.ToString("yyyymmdd") + _logFile.Extension);
            }
        }
    }
}
