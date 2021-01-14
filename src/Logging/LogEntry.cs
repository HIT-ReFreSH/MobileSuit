using System;

namespace PlasticMetal.MobileSuit.Logging
{
    /// <summary>
    ///     A pipeline for LogEntry.
    /// </summary>
    /// <param name="content">Input side LogEntry.</param>
    public delegate LogEntry LogEntryPipeline(in LogEntry content);

    /// <summary>
    ///     An entry in a log file
    /// </summary>
    public struct LogEntry
    {
        /// <summary>
        ///     Initialize a LogEntry with timestamp, type and message
        /// </summary>
        /// <param name="timeStamp">Timestamp of the log</param>
        /// <param name="type">type of the log</param>
        /// <param name="message">message of the log</param>
        public LogEntry(DateTime timeStamp, string type, string message)
        {
            TimeStamp = timeStamp;
            Type = type;
            Message = message;
        }

        /// <summary>
        ///     Timestamp of the log
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     type of the log
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     message of the log
        /// </summary>
        public string Message { get; set; }
    }
}