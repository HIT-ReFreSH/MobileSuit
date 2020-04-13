using PlasticMetal.MobileSuit.ObjectModel.Attributes;

#nullable enable

namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    /// <summary>
    /// Represents a command handler in MobileSuit.
    /// </summary>
    /// <param name="prompt">the prompt display in the Console</param>
    /// <param name="cmd">the command to execute</param>
    /// <returns>TraceBack of this Command</returns>
    public delegate TraceBack CommandHandler(string prompt, string? cmd);
    /// <summary>
    /// Represents that an object is interactive to SuitHost's command handler.
    /// </summary>
    public interface ICommandInteractive
    {
        /// <summary>
        /// Provides Interface for SuitHost to set commandHandler
        /// </summary>
        /// <param name="commandHandler">SuitHost's command handler.</param>
        [SuitIgnore]
        void SetCommandHandler(CommandHandler commandHandler);
    }
}