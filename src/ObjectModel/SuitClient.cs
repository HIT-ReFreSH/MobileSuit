using PlasticMetal.MobileSuit.Logging;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     An Standard mobile suit client driver-class.
    /// </summary>
    public abstract class SuitClient : IInfoProvider, IIOInteractive, ILogInteractive
    {
        /// <summary>
        ///     The information provided.
        /// </summary>
        [SuitIgnore]
        public string Text { get; protected set; } = string.Empty;


        /// <inheritdoc />
        [SuitIgnore]
        public IAssignOnceIOHub IO { get; } = new AssignOnceIOHub();

        /// <inheritdoc />
        [SuitIgnore]
        public IAssignOnceLogger Log { get; } = new AssignOnceLogger();
    }
}