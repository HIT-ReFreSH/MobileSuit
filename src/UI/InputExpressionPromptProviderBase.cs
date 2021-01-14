using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// PromptProvider for input Expression.
    /// </summary>
    public abstract class InputExpressionPromptProviderBase:IPromptProvider
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
        protected InputExpressionPromptProviderBase(IIOHub iOHub)
        {
            InputHelper = iOHub.InputHelper;
            Color = iOHub.ColorSetting;
            Tag = iOHub;
        }
        /// <inheritdoc/>
        public bool Enabled => InputHelper.Expression != null;

        /// <inheritdoc/>
        public abstract bool AsLabel { get; }
        /// <inheritdoc/>
        public object? Tag { get; }

        /// <inheritdoc/>
        public abstract string Content { get; }
        /// <inheritdoc/>
        public ConsoleColor? ForegroundColor => Color.ListTitleColor;

        /// <inheritdoc/>
        public ConsoleColor? BackgroundColor => null;
    }
}
