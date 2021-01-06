using System;
using System.IO;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Logger for MobileSuit
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        ///     get path of current log file
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        ///     Write debug info to the log file
        /// </summary>
        /// <param name="content">content debug info</param>
        public void LogDebug(string content);

        /// <summary>
        ///     Write command info to the log file
        /// </summary>
        /// <param name="content">content command info</param>
        public void LogCommand(string content);


        /// <summary>
        ///     Write return info to the log file
        /// </summary>
        /// <param name="content">return info (TraceBack)</param>
        /// <param name="returnValue">return info (Return Value)</param>
        public void LogTraceBack(TraceBack content, object? returnValue = null);

        /// <summary>
        ///     Write custom exception info to the log file
        /// </summary>
        /// <param name="content">custom exception info</param>
        public void LogException(string content);

        /// <summary>
        ///     Write exception info to the log file
        /// </summary>
        /// <param name="content">content the Exception</param>
        public void LogException(Exception content);

        /// <summary>
        ///     Write debug info to the log file
        /// </summary>
        /// <param name="content">content debug info</param>
        public Task LogDebugAsync(string content);


        /// <summary>
        ///     Write command info to the log file
        /// </summary>
        /// <param name="content">content command info</param>
        public Task LogCommandAsync(string content);


        /// <summary>
        ///     Write return info to the log file
        /// </summary>
        /// <param name="content">return info (TraceBack)</param>
        /// <param name="returnValue">return info (Return Value)</param>
        public Task LogTraceBackAsync(TraceBack content, object? returnValue = null);


        /// <summary>
        ///     Write custom exception info to the log file
        /// </summary>
        /// <param name="content">custom exception info</param>
        public Task LogExceptionAsync(string content);


        /// <summary>
        ///     Write exception info to the log file
        /// </summary>
        /// <param name="content">content the Exception</param>
        public Task LogExceptionAsync(Exception content);

        private static string GetFileName()
        {
            return $"PlasticMetal.MobileSuit_{Environment.ProcessId}_{DateTime.Now.ToFileTime()}.log";
        }

        /// <summary>
        ///     Create a log file in given directory with standard name
        /// </summary>
        /// <param name="dirPath">The directory which the log file will be in</param>
        /// <returns>the log file</returns>
        public static ILogger OfDirectory(string dirPath)
        {
            return Create(Path.Combine(dirPath, GetFileName()));
        }

        private static ILogger Create(string path)
        {
            return new Logger(path);
        }

        /// <summary>
        ///     Create a log file in given path
        /// </summary>
        /// <param name="path">Path of the log file</param>
        /// <returns>the log file</returns>
        public static ILogger OfFile(string path)
        {
            return Create(path);
        }

        /// <summary>
        ///     Create a log file in current directory with standard name
        /// </summary>
        /// <returns>the log file</returns>
        public static ILogger OfLocal()
        {
            return Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                GetFileName()));
        }

        /// <summary>
        ///     Create a log file in current directory with standard name
        /// </summary>
        /// <returns>the log file</returns>
        public static ILogger OfEmpty()
        {
            return new EmptyLogger();
        }

        /// <summary>
        ///     Create a log file in System temp directory with standard name
        /// </summary>
        /// <returns>the log file</returns>
        public static ILogger OfTemp()
        {
            return Create(Path.Combine(Path.GetTempPath(), GetFileName()));
        }
    }
}