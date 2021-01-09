using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI.PowerLine
{
    /// <summary>
    /// Provides a 'Cross' when a non-AllOK TraceBack got.
    /// </summary>
    public class CrossTraceBackPromptProvider:IPromptProvider
    {
        private IColorSetting Color { get; }
        private IHostStatus HostStatus { get; }
        /// <summary>
        /// Initialize a TraceBackPromptProvider with colors.
        /// </summary>
        /// <param name="host">Host to provide TraceBack Prompt for.</param>
        public CrossTraceBackPromptProvider(IMobileSuitHost host)
        {
            HostStatus = host.HostStatus;
            Color = host.IO.ColorSetting;

        }
        /// <inheritdoc/>
        public bool Enabled => HostStatus.TraceBack != TraceBack.AllOk;
        /// <inheritdoc/>
        public bool AsLabel => false;
        /// <inheritdoc/>
        public object? Tag => null;

        /// <inheritdoc/>
        public string Content => " ⨯";

        /// <inheritdoc/>
        public ConsoleColor? ForegroundColor => null;

        /// <inheritdoc/>
        public ConsoleColor? BackgroundColor => Color.ErrorColor;
    }
}
