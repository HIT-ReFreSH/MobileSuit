using System;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Color settings of a Mobile Suit.
    /// </summary>
    public interface IColorSetting : IEquatable<IColorSetting>
    {
        /// <summary>
        /// BackgroundColor
        /// </summary>
        ConsoleColor BackgroundColor{ get; set; }
        /// <summary>
        ///     Default color. For OutputType.Default
        /// </summary>
        ConsoleColor DefaultColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Prompt
        /// </summary>
        ConsoleColor PromptColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Error
        /// </summary>
        ConsoleColor ErrorColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.OK
        /// </summary>
        ConsoleColor OkColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Title
        /// </summary>
        ConsoleColor TitleColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Info
        /// </summary>
        ConsoleColor InformationColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.System
        /// </summary>
        ConsoleColor SystemColor { get; set; }
        /// <summary>
        ///     Prompt Color. For OutputType.System
        /// </summary>
        ConsoleColor WarningColor { get; set; }

        /// <summary>
        ///     Default color settings for IOServer.
        /// </summary>
        public static IColorSetting DefaultColorSetting => new ColorSetting
        {
            DefaultColor = ConsoleColor.White,
            ErrorColor = ConsoleColor.Red,
            PromptColor = ConsoleColor.Magenta,
            OkColor = ConsoleColor.Green,
            TitleColor = ConsoleColor.DarkYellow,
            InformationColor = ConsoleColor.DarkCyan,
            SystemColor = ConsoleColor.DarkBlue,
            WarningColor=ConsoleColor.Yellow,
            BackgroundColor=ConsoleColor.Black
        };


        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="that">The object to compare with the current instance.</param>
        /// <returns>whether this instance and a specified object are equal.</returns>
        bool Equals(object that);

        /// <summary>
        ///     select color for the output type from color setting
        /// </summary>
        /// <param name="colorSetting">color setting</param>
        /// <param name="type">output type</param>
        /// <param name="customColor">customized color</param>
        /// <returns></returns>
        static ConsoleColor SelectColor(IColorSetting colorSetting, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            return
                customColor
                ?? type switch
                {
                    OutputType.Default => colorSetting.DefaultColor,
                    OutputType.Prompt => colorSetting.PromptColor,
                    OutputType.Error => colorSetting.ErrorColor,
                    OutputType.Ok => colorSetting.OkColor,
                    OutputType.Title => colorSetting.TitleColor,
                    OutputType.Info => colorSetting.InformationColor,
                    OutputType.System => colorSetting.SystemColor,
                    OutputType.Warning => colorSetting.WarningColor,
                    _ => colorSetting.BackgroundColor
                };
        }
    }
}