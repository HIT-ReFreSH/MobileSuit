using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel
{

    /// <summary>
    ///     An Standard mobile suit client driver-class.
    /// </summary>
    public abstract class SuitClient : IInfoProvider, IIOInteractive, ILogInteractive
    {


        /// <inheritdoc />
        [SuitIgnore]
        public IAssignOnceIOServer IO { get; } = new AssignOnceIOServer();


        /// <summary>
        ///     The information provided.
        /// </summary>
        [SuitIgnore]
        public string Text { get; protected set; } = string.Empty;

        /// <inheritdoc />
        [SuitIgnore]
        public IAssignOnceLogger Log { get; } = new AssignOnceLogger();
    }
}