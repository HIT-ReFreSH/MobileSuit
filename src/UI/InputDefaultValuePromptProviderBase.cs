using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// PromptProvider for input DefaultValue.
    /// </summary>
    public abstract class InputDefaultValuePromptProviderBase:IPromptProvider
    {
        /// <summary>
        /// ColorSetting
        /// </summary>
        protected IColorSetting Color { get; }
        /// <summary>
        /// Information for input.
        /// </summary>
        protected IInputHelper InputHelper { get; }
        /// <summary>
        /// Initialize a TraceBackPromptProvider with colors.
        /// </summary>
        /// <param name="iOHub">IOHub this providing info for.</param>
        protected InputDefaultValuePromptProviderBase(IIOHub iOHub)
        {
            InputHelper = iOHub.InputHelper;
            Color = iOHub.ColorSetting;
            Tag = iOHub;
        }
        /// <inheritdoc/>
        public bool Enabled => InputHelper.DefaultInput != null;
        /// <inheritdoc/>
        public abstract bool AsLabel { get; }


        /// <inheritdoc/>
        public object? Tag { get; }
        /// <inheritdoc/>
        public abstract string Content { get; }

        /// <inheritdoc/>
        public ConsoleColor? ForegroundColor => Color.CustomInformationColor;

        /// <inheritdoc/>
        public ConsoleColor? BackgroundColor => null;
    }
}
