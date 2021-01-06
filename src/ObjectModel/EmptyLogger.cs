using System;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     Empty Logger for MobileSuit, do nothing when called.
    /// </summary>
    public class EmptyLogger : ILogger
    {
        /// <inheritdoc />
        public void LogDebug(string content)
        {
        }

        /// <inheritdoc />
        public void LogCommand(string content)
        {
        }


        /// <inheritdoc />
        public void LogTraceBack(TraceBack content, object? returnValue = null)
        {
        }

        /// <inheritdoc />
        public void LogException(string content)
        {
        }

        /// <inheritdoc />
        public void LogException(Exception content)
        {
        }

        /// <inheritdoc />
        public Task LogDebugAsync(string content)
        {
            return Task.CompletedTask;
        }


        /// <inheritdoc />
        public Task LogCommandAsync(string content)
        {
            return Task.CompletedTask;
        }


        /// <inheritdoc />
        public Task LogTraceBackAsync(TraceBack content, object? returnValue = null)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task LogExceptionAsync(string content)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task LogExceptionAsync(Exception content)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Do nothing, because EmptyLogger has nothing to release.
        /// </summary>
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public string FilePath => "";
    }
}