using System;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.UI;

namespace PlasticMetal.MobileSuit.UI.PowerLine
{
    /// <summary>
    /// PromptProvider for TraceBack information.
    /// </summary>
    public class TraceBackPromptProvider:IPromptProvider
    {
        private IColorSetting Color { get; }
        private IHostStatus HostStatus { get; }
        /// <summary>
        /// Initialize a TraceBackPromptProvider with colors.
        /// </summary>
        /// <param name="host">Host to provide TraceBack Prompt for.</param>
        public TraceBackPromptProvider(IMobileSuitHost host)
        {
            HostStatus = host.HostStatus;
            Color = host.IO.ColorSetting;
            
        }
        /// <inheritdoc/>
        public bool Enabled =>true;
        /// <inheritdoc/>
        public bool AsLabel => true;
        /// <inheritdoc/>
        public object? Tag => null;
        /// <inheritdoc/>
        public string Content =>HostStatus.TraceBack switch
        {
            RequestStatus.AllOk => Lang.AllOK,
            RequestStatus.InvalidCommand => Lang.InvalidCommand,
            RequestStatus.ObjectNotFound => Lang.ObjectNotFound,
            RequestStatus.MemberNotFound => Lang.MemberNotFound,
            _ => ""
        };
        /// <inheritdoc/>
        public ConsoleColor? ForegroundColor=> IColorSetting.SelectColor(Color,
            HostStatus.TraceBack == 0 ? OutputType.AllOk : OutputType.Error);
        /// <inheritdoc/>
        public ConsoleColor? BackgroundColor => null;
    }
}
