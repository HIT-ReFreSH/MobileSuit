using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.Logging
{
    /// <summary>
    ///     A Logger can be assigned once
    /// </summary>
    public interface IAssignOnceLogger : ISuitLogger, IAssignOnce<ISuitLogger>
    {
    }

    /// <summary>
    ///     A Logger can be assigned once
    /// </summary>
    public class AssignOnceLogger : IAssignOnceLogger
    {
        /// <summary>
        ///     The real logger used
        /// </summary>
        protected ISuitLogger Element { get; private set; } = ISuitLogger.CreateEmpty();

        /// <inheritdoc />
        public void Append(in LogEntry content)
        {
            Element.Append(content);
        }

        /// <inheritdoc />
        public string Address => Element.Address;

        /// <inheritdoc />
        public void Dispose()
        {
            Element.Dispose();
        }

        /// <inheritdoc />
        public void Assign(ISuitLogger t)
        {
            Element.Dispose();
            Element = t;
        }
    }
}