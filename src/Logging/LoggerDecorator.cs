namespace PlasticMetal.MobileSuit.Logging
{
    /// <summary>
    ///     Decorator Pattern for <c>ISuitLogger</c>.
    /// </summary>
    public abstract class LoggerDecorator : ISuitLogger
    {
        /// <summary>
        ///     Initialize a logger decorator with decorated Logger.
        /// </summary>
        /// <param name="logger"></param>
        protected LoggerDecorator(ISuitLogger logger)
        {
            Decorated = logger;
        }

        private ISuitLogger Decorated { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Decorated.Dispose();
        }

        /// <inheritdoc />
        public void Append(in LogEntry content)
        {
            Decorated.Append(Decorate(content));
        }

        /// <inheritdoc />
        public string Address => Decorated.Address;

        /// <summary>
        ///     The decorator method, performs on each LogEntry passed to <c>Append</c>.
        /// </summary>
        /// <param name="entry">Input LogEntry passed to <c>Append</c>.</param>
        /// <returns>Output LogEntry passing to <c>Append</c> of decorated Logger.</returns>
        protected abstract LogEntry Decorate(in LogEntry entry);
    }
}