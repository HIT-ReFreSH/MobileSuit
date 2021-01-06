namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     Represents that an object is interactive to SuitHost's IOServer
    /// </summary>
    public interface IIOInteractive
    {
        /// <summary>
        ///     Provides Interface for SuitHost to set ioServer
        /// </summary>
        [SuitIgnore]
        IAssignOnceIOServer IO { get; }
    }
}