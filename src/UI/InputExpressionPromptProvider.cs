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
    public class InputExpressionPromptProvider:IPromptProvider
    {
        private IColorSetting Color { get; }
        private IInputHelper InputHelper { get; }
        /// <summary>
        /// Initialize a TraceBackPromptProvider with colors.
        /// </summary>
        /// <param name="iOHub">IOHub this providing info for.</param>
        public InputExpressionPromptProvider(IIOHub iOHub)
        {
            InputHelper = iOHub.InputHelper;
            Color = iOHub.ColorSetting;
            Tag = iOHub;
        }
        /// <inheritdoc/>
        public bool Enabled => InputHelper.Expression != null;

        /// <inheritdoc/>
        public bool AsLabel => false;
        /// <inheritdoc/>
        public object? Tag { get; }

        /// <inheritdoc/>
        public string Content => $" {InputHelper.Expression ?? ""}";
        /// <inheritdoc/>
        public ConsoleColor? ForegroundColor => Color.ListTitleColor;

        /// <inheritdoc/>
        public ConsoleColor? BackgroundColor => null;
    }
}
