using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// An Standard mobile suit client driver-class.
    /// </summary>
    public abstract class MsClient : IInfoProvider, IIoInteractive, ICommandInteractive
    {
        /// <summary>
        /// The CommandHandler for current MsHost.
        /// </summary>
        protected CommandHandler RunCommand { get; private set; }
        /// <summary>
        /// The IoServer for current MsHost.
        /// </summary>
        protected IoServer Io { get; private set; }
        /// <summary>
        /// Provides Interface for MsHost to set commandHandler
        /// </summary>
        /// <param name="commandHandler">MsHost's command handler.</param>
        [MsIgnorable]
        public void SetCommandHandler(CommandHandler commandHandler)
        {
            RunCommand = commandHandler;
        }
        /// <summary>
        /// The information provided.
        /// </summary>
        public string Text { get; protected set; }
        /// <summary>
        /// Provides Interface for MsHost to set ioServer
        /// </summary>
        /// <param name="io">MsHost's IoServer.</param>
        [MsIgnorable]
        public void SetIo(IoServer io)
        {
            Io = io;
        }
    }
}