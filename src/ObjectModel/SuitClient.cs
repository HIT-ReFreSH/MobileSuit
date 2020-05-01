using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// An Standard mobile suit client driver-class.
    /// </summary>
    public abstract class SuitClient : IInfoProvider, IIOInteractive, ICommandInteractive
    {
        /// <summary>
        /// The CommandHandler for current SuitHost.
        /// </summary>
        protected CommandHandler? RunCommand { get; private set; }

        /// <summary>
        /// The IOServer for current SuitHost.
        /// </summary>
        protected IIOServer IO { get; private set; } = IIOServer.GeneralIO;
        /// <summary>
        /// Provides Interface for SuitHost to set commandHandler
        /// </summary>
        /// <param name="commandHandler">SuitHost's command handler.</param>
        [SuitIgnore]
        public void SetCommandHandler(CommandHandler commandHandler)
        {
            RunCommand = commandHandler;
        }

        /// <summary>
        /// The information provided.
        /// </summary>
        public string Text { get; protected set; } = string.Empty;
        /// <summary>
        /// Provides Interface for SuitHost to set ioServer
        /// </summary>
        /// <param name="io">SuitHost's IOServer.</param>
        [SuitIgnore]
        public void SetIO(IIOServer io)
        {
            IO = io;
        }
    }
}