using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// Logger for MobileSuit
    /// </summary>
    public interface ILogger
    {

        /// <summary>
        /// Write debug info to the log file
        /// </summary>
        /// <param name="content">content debug info</param>
        public void LogDebug(string content);

        /// <summary>
        /// Write command info to the log file
        /// </summary>
        /// <param name="content">content command info</param>
        public void LogCommand(string content);


        /// <summary>
        /// Write return info to the log file
        /// </summary>
        /// <param name="content">return info (TraceBack)</param>
        /// <param name="returnValue">return info (Return Value)</param>
        public void LogTraceBack(TraceBack content, object? returnValue = null);

        /// <summary>
        /// Write custom exception info to the log file
        /// </summary>
        /// <param name="content">custom exception info</param>
        public void LogException(string content);

        /// <summary>
        /// Write exception info to the log file
        /// </summary>
        /// <param name="content">content the Exception</param>
        public void LogException(Exception content);

        /// <summary>
        /// Write debug info to the log file
        /// </summary>
        /// <param name="content">content debug info</param>
        public Task LogDebugAsync(string content);


        /// <summary>
        /// Write command info to the log file
        /// </summary>
        /// <param name="content">content command info</param>
        public Task LogCommandAsync(string content);



        /// <summary>
        /// Write return info to the log file
        /// </summary>
        /// <param name="content">return info (TraceBack)</param>
        /// <param name="returnValue">return info (Return Value)</param>
        public Task LogTraceBackAsync(TraceBack content, object? returnValue = null);


        /// <summary>
        /// Write custom exception info to the log file
        /// </summary>
        /// <param name="content">custom exception info</param>
        public Task LogExceptionAsync(string content);


        /// <summary>
        /// Write exception info to the log file
        /// </summary>
        /// <param name="content">content the Exception</param>
        public Task LogExceptionAsync(Exception content);
        /// <summary>
        /// get path of current log file
        /// </summary>
        public string FilePath { get; }
        private static string GetFileName()
        {
            return $"PlasticMetal.MobileSuit_{Process.GetCurrentProcess().Id}_{DateTime.Now.ToFileTime()}.log";
        }

        /// <summary>
        /// Create a log file in given directory with standard name
        /// </summary>
        /// <param name="dirPath">The directory which the log file will be in</param>
        /// <returns>the log file</returns>
        public static Logger OfDirectory(string dirPath)
        {
            return Create(Path.Combine(dirPath, GetFileName()));
        }

        private static Logger Create(string path)
        {
            return new Logger(path);

        }

        /// <summary>
        /// Create a log file in given path
        /// </summary>
        /// <param name="path">Path of the log file</param>
        /// <returns>the log file</returns>
        public static Logger OfFile(string path)
        {
            return Create(path);
        }

        /// <summary>
        /// Create a log file in current directory with standard name
        /// </summary>
        /// <returns>the log file</returns>
        public static Logger OfLocal()
        {

            return Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GetFileName()));
        }

        /// <summary>
        /// Create a log file in System temp directory with standard name
        /// </summary>
        /// <returns>the log file</returns>
        public static Logger OfTemp()
        {

            return Create(Path.Combine(Path.GetTempPath(), GetFileName()));
        }
    }
}
