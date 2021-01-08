namespace PlasticMetal.MobileSuit.Logging
{
    internal class DecoratedLogger : LoggerDecorator
    {
        public DecoratedLogger(ISuitLogger logger, LogEntryPipeline decorator) : base(logger)
        {
            Pipeline = decorator;
        }

        private LogEntryPipeline Pipeline { get; }

        protected override LogEntry Decorate(in LogEntry entry)
        {
            return Pipeline(entry);
        }
    }
}