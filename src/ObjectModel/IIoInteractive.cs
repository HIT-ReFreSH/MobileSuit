using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// Represents that an object is interactive to SuitHost's IOServer
    /// </summary>
    public interface IIOInteractive
    {
        /// <summary>
        /// Provides Interface for SuitHost to set ioServer
        /// </summary>
        /// <param name="io">SuitHost's IOServer.</param>
        [SuitIgnore]
        void SetIO(IIOServer io);
    }
}