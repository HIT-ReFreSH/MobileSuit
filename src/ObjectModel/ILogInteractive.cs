using PlasticMetal.MobileSuit.Core.Logging;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     has access to host's logger
    /// </summary>
    public interface ILogInteractive
    {
        /// <summary>
        ///     Provides Interface for SuitHost to set Logger
        /// </summary>
        [SuitIgnore]
        IAssignOnceLogger Log { get; }
    }
}