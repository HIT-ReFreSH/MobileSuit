using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.IO;

namespace PlasticMetal.MobileSuit.ObjectModel
{

    /// <summary>
    ///     An Standard mobile suit client driver-class.
    /// </summary>
    public abstract class SuitClient : IInfoProvider, IIOInteractive, IHostInteractive
    {


        /// <inheritdoc />
        [SuitIgnore]
        public IAssignOnceIOServer IO { get; } = new AssignOnceIOServer();


        /// <summary>
        ///     The information provided.
        /// </summary>
        public string Text { get; protected set; } = string.Empty;

        /// <inheritdoc />
        [SuitIgnore]
        public IAssignOnceHost Host { get; } = new AssignOnceHost();
    }
}