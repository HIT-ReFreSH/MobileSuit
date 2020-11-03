using PlasticMetal.MobileSuit.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public class AssignOnceLogger :  IAssignOnceLogger,IDisposable
    {
        
        /// <inheritdoc />
        public void LogDebug(string content)
            => Element.LogDebug(content);
        /// <inheritdoc />
        public void LogCommand(string content)
            => Element.LogCommand(content);
        /// <inheritdoc />
        public void LogTraceBack(TraceBack content, object? returnValue = null)
            => Element.LogTraceBack(content, returnValue);
        /// <inheritdoc />
        public void LogException(string content)
            => Element.LogException(content);
        /// <inheritdoc />
        public void LogException(Exception content)
            => Element.LogException(content);
        /// <inheritdoc />
        public Task LogDebugAsync(string content)
            => Element.LogDebugAsync(content);
        /// <inheritdoc />
        public Task LogCommandAsync(string content)
            => Element.LogCommandAsync(content);
        /// <inheritdoc />
        public Task LogTraceBackAsync(TraceBack content, object? returnValue = null)
            => Element.LogTraceBackAsync(content, returnValue);
        /// <inheritdoc />
        public Task LogExceptionAsync(string content)
            => Element.LogExceptionAsync(content);
        /// <inheritdoc />
        public Task LogExceptionAsync(Exception content)
            => Element.LogExceptionAsync(content);

        /// <inheritdoc />
        public string FilePath => (Element).FilePath;
        /// <inheritdoc/>
        public void Dispose()
            => Element.Dispose();

        /// <summary>
        /// The real logger used
        /// </summary>
        protected ILogger Element { get; private set; } = ILogger.OfTemp();
        /// <inheritdoc/>
        public void Assign(ILogger t)
        {
            Element.Dispose();
            Element = t;
        }
    }
}
