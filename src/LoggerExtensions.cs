using System;
using System.Diagnostics;
using System.Text;
using PlasticMetal.MobileSuit.Logging;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Useful Extension Methods for SuitLogger.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        ///     Get a <c>CachedLogger</c> of given logger.
        /// </summary>
        /// <param name="logger">Logger to cache.</param>
        /// <returns>
        ///     <c>CachedLogger</c>
        /// </returns>
        public static ISuitLogger AsCached(this ISuitLogger logger)
        {
            return new CachedLogger(logger);
        }

        /// <summary>
        ///     Get a decorated logger of given logger.
        /// </summary>
        /// <param name="logger">Logger to decorate.</param>
        /// <param name="decorator">Decorator for the logger.</param>
        /// <returns>Decorated logger.</returns>
        public static ISuitLogger DecorateWith(this ISuitLogger logger, LogEntryPipeline decorator)
        {
            return new DecoratedLogger(logger, decorator);
        }

        /// <summary>
        ///     Write debug info to Logger
        /// </summary>
        /// <param name="logger">Logger to write into.</param>
        /// <param name="tag">Tag for this log.</param>
        /// <param name="content">content debug info</param>
        public static void LogDebug(this ISuitLogger logger, string tag, string content)
        {
            logger.WriteLog(tag, content);
        }

        /// <summary>
        ///     Write debug info to Logger
        /// </summary>
        /// <param name="logger">Logger to write into.</param>
        /// <param name="content">content debug info</param>
        public static void LogInformation(this ISuitLogger logger, string content)
        {
            logger.WriteLog("Info", content);
        }

        /// <summary>
        ///     Write command info to Logger
        /// </summary>
        /// <param name="logger">Logger to write into.</param>
        /// <param name="content">content command info</param>
        public static void LogCommand(this ISuitLogger logger, string content)
        {
            logger.WriteLog("Command", content);
        }


        /// <summary>
        ///     Write return info to Logger
        /// </summary>
        /// <param name="logger">Logger to write into.</param>
        /// <param name="content">return info (TraceBack)</param>
        /// <param name="returnValue">return info (Return Value)</param>
        public static void LogTraceBack(this ISuitLogger logger, RequestStatus content, object? returnValue = null)
        {
            logger.WriteLog("TraceBack", content + returnValue switch
            {
                { } => $"({returnValue})",
                null => ""
            });
        }

        /// <summary>
        ///     Write custom exception info to Logger.
        /// </summary>
        /// <param name="logger">Logger to write into.</param>
        /// <param name="content">custom exception info.</param>
        public static void LogException(this ISuitLogger logger, string content)
        {
            logger.WriteLog("Exception", content);
        }

        /// <summary>
        ///     Write exception info to Logger
        /// </summary>
        /// <param name="logger">Logger to write into.</param>
        /// <param name="content">content the Exception</param>
        public static void LogException(this ISuitLogger logger, Exception content)
        {
            StringBuilder stringBuilder = new StringBuilder(content.Message);
            foreach (var se in new StackTrace(content).GetFrames()) stringBuilder.Append("\n\tAt ").Append(se);
            logger.WriteLog(content.GetType().Name, stringBuilder.ToString());
        }


        private static void WriteLog(this ISuitLogger logger, string info, string content)
        {
            logger.Append(new LogEntry(DateTime.Now, info, content));
        }
    }
}