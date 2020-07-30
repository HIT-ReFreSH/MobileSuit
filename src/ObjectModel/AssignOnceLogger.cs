using PlasticMetal.MobileSuit.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// A Logger can be assigned once
    /// </summary>
    public interface IAssignOnceLogger:ILogger,IAssignOnce<ILogger>
    {
    }
    /// <summary>
    /// A Logger can be assigned once
    /// </summary>
    public class AssignOnceLogger : AssignOnce<ILogger>, IAssignOnceLogger
    {
        /// <inheritdoc />
        public void LogDebug(string content)
            => (Element ?? ILogger.OfTemp()).LogDebug(content);
        /// <inheritdoc />
        public void LogCommand(string content)
            => (Element ?? ILogger.OfTemp()).LogCommand(content);
        /// <inheritdoc />
        public void LogTraceBack(TraceBack content, object? returnValue = null)
            => (Element ?? ILogger.OfTemp()).LogTraceBack(content, returnValue);
        /// <inheritdoc />
        public void LogException(string content)
            => (Element ?? ILogger.OfTemp()).LogException(content);
        /// <inheritdoc />
        public void LogException(Exception content)
            => (Element ?? ILogger.OfTemp()).LogException(content);
        /// <inheritdoc />
        public Task LogDebugAsync(string content)
            => (Element ?? ILogger.OfTemp()).LogDebugAsync(content);
        /// <inheritdoc />
        public Task LogCommandAsync(string content)
            => (Element ?? ILogger.OfTemp()).LogCommandAsync(content);
        /// <inheritdoc />
        public Task LogTraceBackAsync(TraceBack content, object? returnValue = null)
            => (Element ?? ILogger.OfTemp()).LogTraceBackAsync(content, returnValue);
        /// <inheritdoc />
        public Task LogExceptionAsync(string content)
            => (Element ?? ILogger.OfTemp()).LogExceptionAsync(content);
        /// <inheritdoc />
        public Task LogExceptionAsync(Exception content)
            => (Element ?? ILogger.OfTemp()).LogExceptionAsync(content);

        /// <inheritdoc />
        public string FilePath => (Element ?? ILogger.OfTemp()).FilePath;
    }
}
