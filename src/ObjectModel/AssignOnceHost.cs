using PlasticMetal.MobileSuit.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// A MobileSuitHost, which can only be assigned once
    /// </summary>
    public interface IAssignOnceHost : IAssignOnce<IMobileSuitHost>, IMobileSuitHost { }
    /// <summary>
    /// A MobileSuitHost, which can only be assigned once
    /// </summary>
    public class AssignOnceHost : AssignOnce<IMobileSuitHost>, IAssignOnceHost
    {
        /// <inheritdoc />
        public HostSettings Settings
        {
            get => Element?.Settings ?? new HostSettings();
            set
            {
                if (Element != null)
                {
                    Element.Settings = value;
                }
            }
        }

        /// <inheritdoc />
        public int Run(string prompt) => Element?.Run(prompt) ?? -1;
        /// <inheritdoc />
        public int Run() => Element?.Run() ?? -1;
        /// <inheritdoc />
        public Task<TraceBack> RunScriptsAsync(IAsyncEnumerable<string?> scripts, bool withPrompt = false, string? scriptName = null)
        => Element?.RunScriptsAsync(scripts, withPrompt, scriptName) ??
            Task.Run(() =>
        {
            return TraceBack.ObjectNotFound;
        });
        /// <inheritdoc />
        public TraceBack RunScripts(IEnumerable<string> scripts, bool withPrompt = false, string? scriptName = null)
            => Element?.RunScripts(scripts, withPrompt, scriptName) ?? TraceBack.ObjectNotFound;
        /// <inheritdoc />
        public TraceBack RunCommand(string? command, string prompt = "")
            => Element?.RunCommand(command, prompt) ?? TraceBack.ObjectNotFound;
        /// <inheritdoc />
        public int Run(string[] args) => Element?.Run(args) ?? -1;


        /// <inheritdoc />
        public Logger Logger => Element?.Logger ?? ILogger.OfTemp();

        /// <inheritdoc />
        public IIOServer IO => Element?.IO ?? IIOServer.GeneralIO;
    }
}
