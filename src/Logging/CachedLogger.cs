using System.Collections;
using System.Collections.Generic;

namespace PlasticMetal.MobileSuit.Logging
{
    /// <summary>
    ///     A memory-cached Logger.
    /// </summary>
    public class CachedLogger : LoggerDecorator, IEnumerable<LogEntry>
    {
        /// <summary>
        ///     Initialize a memory-cached Logger with the base logger to cache.
        /// </summary>
        /// <param name="logger">Base logger to cache.</param>
        public CachedLogger(ISuitLogger logger) : base(logger)
        {
        }

        private List<LogEntry> Logs { get; } = new List<LogEntry>();

        /// <summary>
        ///     Count of logs cached.
        /// </summary>
        public int Count => Logs.Count;

        /// <inheritdoc></inheritdoc>
        public IEnumerator<LogEntry> GetEnumerator()
        {
            return Logs.GetEnumerator();
        }

        /// <inheritdoc></inheritdoc>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc></inheritdoc>
        protected override LogEntry Decorate(in LogEntry entry)
        {
            Logs.Add(entry);
            return entry;
        }

        /// <summary>
        ///     Clear the cached Logs.
        /// </summary>
        public void Clear()
        {
            Logs.Clear();
        }
    }
}