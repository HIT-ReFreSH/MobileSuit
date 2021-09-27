using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// Providing Return value information to prompt.
    /// </summary>
    public class ReturnValuePromptProvider : IPromptProvider
    {
        private IColorSetting Color { get; }
        private IHostStatus HostStatus { get; }
        /// <summary>
        /// Initialize a TraceBackPromptProvider with colors.
        /// </summary>
        /// <param name="host">Host to provide TraceBack Prompt for.</param>
        public ReturnValuePromptProvider(IMobileSuitHost host)
        {
            HostStatus = host.HostStatus;
            Color = host.IO.ColorSetting;

        }
        /// <inheritdoc/>
        public bool Enabled => HostStatus.ReturnValue != null;

        /// <inheritdoc/>
        public bool AsLabel => true;

        /// <inheritdoc/>
        public object? Tag => null;
        /// <inheritdoc/>
        public string Content => HostStatus.ReturnValue?.ToString() ?? "";

        /// <inheritdoc/>
        public ConsoleColor? ForegroundColor => Color.SystemColor;

        /// <inheritdoc/>
        public ConsoleColor? BackgroundColor => null;
    }
}
