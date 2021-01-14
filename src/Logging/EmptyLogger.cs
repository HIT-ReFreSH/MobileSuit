namespace PlasticMetal.MobileSuit.Logging
{
    /// <summary>
    ///     Empty Logger for MobileSuit, do nothing when called.
    /// </summary>
    public class EmptyLogger : ISuitLogger
    {
        /// <inheritdoc />
        public void Append(in LogEntry content)
        {
        }

        /// <inheritdoc />
        public string Address => string.Empty;

        /// <summary>
        ///     Do nothing, because EmptyLogger has nothing to release.
        /// </summary>
        public void Dispose()
        {
        }
    }
}