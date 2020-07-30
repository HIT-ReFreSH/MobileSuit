using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// An entry in a log file
    /// </summary>
    public class LogEntry
    {

        /// <summary>
        /// Timestamp of the log
        /// </summary>
        public DateTime TimeStamp{ get; set; }
        /// <summary>
        /// type of the log
        /// </summary>
        public string Type{ get; set; }
        /// <summary>
        /// message of the log
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Initialize a LogEntry with timestamp, type and message
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
    }
}
