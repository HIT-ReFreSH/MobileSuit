#nullable enable

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     Represents a command handler in MobileSuit.
    /// </summary>
    /// <param name="prompt">the prompt display in the Console</param>
    /// <param name="cmd">the command to execute</param>
    /// <returns>TraceBack of this Command</returns>
    public delegate RequestStatus CommandHandler(string prompt, string? cmd);

    /// <summary>
    ///     Represents that an object is interactive to SuitHost's command handler.
    /// </summary>
    public interface IHostInteractive
    {
        /// <summary>
        ///     Provides Interface for SuitHost.
        /// </summary>
        [SuitIgnore]
        IAssignOnceHost Host { get; }
    }
}