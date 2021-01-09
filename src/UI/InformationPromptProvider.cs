using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// PromptProvider for Instance information.
    /// </summary>
    public class InformationPromptProvider : IPromptProvider
    {
        private Type Type { get; }
        private object Instance { get; }
        /// <summary>
        /// PromptProvider for Instance information.
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <param name="color">Color setting</param>
        public InformationPromptProvider(object instance, IColorSetting color)
        {
            Type = instance.GetType();
            Instance = instance;
            Color = color;
        }

        private IColorSetting Color { get; }

        /// <inheritdoc/>
        public bool Enabled => true;
        /// <inheritdoc/>
        public bool AsLabel => true;

        /// <inheritdoc/>
        public object? Tag => null;
        /// <inheritdoc/>
        public string Content => Type != null
            ? (Type.GetCustomAttribute(typeof(SuitInfoAttribute)) as SuitInfoAttribute)?.Text ??
              (Instance as IInfoProvider)?.Text ??
              new SuitInfoAttribute(Type.Name).Text
            : "";

        /// <inheritdoc/>
        public ConsoleColor? ForegroundColor => Color.PromptColor;
        /// <inheritdoc/>
        public ConsoleColor? BackgroundColor => null;
    }
}
