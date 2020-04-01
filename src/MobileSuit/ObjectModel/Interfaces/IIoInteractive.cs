using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    /// <summary>
    /// Represents that an object is interactive to MsHost's IoServer
    /// </summary>
    public interface IIoInteractive
    {
        /// <summary>
        /// Provides Interface for MsHost to set ioServer
        /// </summary>
        /// <param name="io">MsHost's IoServer.</param>
        [MsIgnorable]
        void SetIo(IoServer io);
    }
}