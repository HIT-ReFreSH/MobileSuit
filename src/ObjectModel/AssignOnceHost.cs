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
        private class DefaultHostStatus : IHostStatus
        {
            public RequestStatus TraceBack => RequestStatus.AllOk;
            public object? ReturnValue => null;
        }
        /// <inheritdoc />
        public IHostStatus HostStatus => Element?.HostStatus ?? new DefaultHostStatus();

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
        public Task<RequestStatus> RunScriptsAsync(IAsyncEnumerable<string?> scripts, bool withPrompt = false,
            string? scriptName = null)
        {
            return Element?.RunScriptsAsync(scripts, withPrompt, scriptName) ??
                   Task.Run(() => RequestStatus.ObjectNotFound);
        }

        /// <inheritdoc />
        public RequestStatus RunScripts(IEnumerable<string> scripts, bool withPrompt = false, string? scriptName = null)
        {
            return Element?.RunScripts(scripts, withPrompt, scriptName) ?? RequestStatus.ObjectNotFound;
        }

        /// <inheritdoc />
        public RequestStatus RunCommand(string? command)
        {
            return Element?.RunCommand(command) ?? RequestStatus.ObjectNotFound;
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