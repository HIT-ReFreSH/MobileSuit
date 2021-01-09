using System.Collections.Generic;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Logging;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     A MobileSuitHost, which can only be assigned once
    /// </summary>
    public interface IAssignOnceHost : IAssignOnce<IMobileSuitHost>, IMobileSuitHost
    {
    }

    /// <summary>
    ///     A MobileSuitHost, which can only be assigned once
    /// </summary>
    public class AssignOnceHost : AssignOnce<IMobileSuitHost>, IAssignOnceHost
    {
        /// <inheritdoc />
        public IHostStatus HostStatus => Element?.HostStatus ??
                                         (IHostStatus)new {Retur =new object(), TraceBack = TraceBack.AllOk};

        /// <inheritdoc />
        public HostSettings Settings
        {
            get => Element?.Settings ?? new HostSettings();
            set
            {
                if (Element != null) Element.Settings = value;
            }
        }


        /// <inheritdoc />
        public int Run()
        {
            return Element?.Run() ?? -1;
        }

        /// <inheritdoc />
        public Task<TraceBack> RunScriptsAsync(IAsyncEnumerable<string?> scripts, bool withPrompt = false,
            string? scriptName = null)
        {
            return Element?.RunScriptsAsync(scripts, withPrompt, scriptName) ??
                   Task.Run(() => { return TraceBack.ObjectNotFound; });
        }

        /// <inheritdoc />
        public TraceBack RunScripts(IEnumerable<string> scripts, bool withPrompt = false, string? scriptName = null)
        {
            return Element?.RunScripts(scripts, withPrompt, scriptName) ?? TraceBack.ObjectNotFound;
        }

        /// <inheritdoc />
        public TraceBack RunCommand(string? command)
        {
            return Element?.RunCommand(command) ?? TraceBack.ObjectNotFound;
        }

        /// <inheritdoc />
        public int Run(string[] args)
        {
            return Element?.Run(args) ?? -1;
        }


        /// <inheritdoc />
        public ISuitLogger Logger => Element?.Logger ?? ISuitLogger.CreateEmpty();

        /// <inheritdoc />
        public IIOHub IO => Element?.IO ?? IIOHub.GeneralIO;
    }
}