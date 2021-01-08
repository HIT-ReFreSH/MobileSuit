using System;
using System.IO;

namespace PlasticMetal.MobileSuit.Logging
{
    /// <summary>
    ///     Logger for MobileSuit
    /// </summary>
    public interface ISuitLogger : IDisposable
    {
        /// <summary>
        ///     Where the logs are at.
        /// </summary>
        public string Address { get; }

        /// <summary>
        ///     Write log content described by the LogEntry to Logger.
        /// </summary>
        /// <param name="content">content debug info</param>
        public void Append(in LogEntry content);

        private static string GetFileName()
        {
            return $"PlasticMetal.MobileSuit_{Environment.ProcessId}_{DateTime.Now.ToFileTime()}.log";
        }

        /// <summary>
        ///     Create a logger with log file in given directory with standard name
        /// </summary>
        /// <param name="dirPath">The directory which Logger will be in</param>
        /// <returns>Logger</returns>
        public static ISuitLogger CreateFileByDirectory(string dirPath)
        {
            return CreateFile(Path.Combine(dirPath, GetFileName()));
        }

        private static ISuitLogger CreateFile(string path)
        {
            return new FileLogger(path);
        }

        /// <summary>
        ///     Create a logger with log file in given path.
        /// </summary>
        /// <param name="path">Path of Logger</param>
        /// <returns>Logger</returns>
        public static ISuitLogger CreateFileByPath(string path)
        {
            return CreateFile(path);
        }


        /// <summary>
        ///     Create Empty logger, doing nothing when called. This is the default logger.
        /// </summary>
        /// <returns>Logger</returns>
        public static ISuitLogger CreateEmpty()
        {
            return new EmptyLogger();
        }

        /// <summary>
        ///     Create a logger with log file in current directory with standard name
        /// </summary>
        /// <returns>Logger</returns>
        public static ISuitLogger CreateFile()
        {
            return CreateFile(GetFileName());
        }
    }
}