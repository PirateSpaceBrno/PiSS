﻿using System;
using System.IO;

namespace PiSS
{
    public class Logger
    {
        private static String _logFilePath;
        private FileInfo _logFile;
        private const int logSize = 20 * 1024 * 1024;
        private bool IsDebugRun;

        /// <summary>
        /// Creates an instance of logging engine.
        /// </summary>
        public Logger(string logTitle)
        {
            _logFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{logTitle}.log";

            if (File.Exists(_logFilePath) == false)
            {
                File.Create(_logFilePath);
            }

            _logFile = new FileInfo(_logFilePath);

            IsDebugRun = File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\{logTitle}.DEBUG");
        }

        public enum logLevel
        {
            INFO,
            DEBUG,
            FAIL,
            FATAL
        }

        /// <summary>
        /// Write log message to the log file. If log file doesn't exists, this will create it.
        /// </summary>
        /// <param name="logMessage">Message specifying logged event.</param>
        /// <param name="logLevel">Log level of the message</param>
        /// <param name="exception">If exception is specified, its message and stacktrace is included in the log.</param>
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

            Rollup();

            using (StreamWriter w = new StreamWriter(_logFilePath))
            {
                w.WriteLine(logLine);
            }
        }

        /// <summary>
        /// Applies only if log file exceed maximum size (default 20 MB). It renames current log file to name containing actual date.
        /// </summary>
        private void Rollup(int maximumLogSizeInBytes = logSize)
        {
            if ((_logFile.Exists && _logFile.Length >= logSize))
            {
                File.Move(_logFilePath, _logFile.Name + DateTime.Now.ToString("yyyymmdd") + _logFile.Extension);
            }
        }
    }
}
