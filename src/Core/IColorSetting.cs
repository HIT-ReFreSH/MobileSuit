using System;
using System.Drawing;

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
        Color BackgroundColor{ get; set; }
        /// <summary>
        ///     Default color. For OutputType.Default
        /// </summary>
        Color DefaultColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Prompt
        /// </summary>
        Color PromptColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Error
        /// </summary>
        Color ErrorColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.OK
        /// </summary>
        Color OkColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Title
        /// </summary>
        Color TitleColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Info
        /// </summary>
        Color InformationColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.System
        /// </summary>
        Color SystemColor { get; set; }
        /// <summary>
        ///     Prompt Color. For OutputType.System
        /// </summary>
        Color WarningColor { get; set; }

        /// <summary>
        ///     Default color settings for IOServer.
        /// </summary>
        public static IColorSetting DefaultColorSetting => new ColorSetting
        {
            DefaultColor = Color.White,
            ErrorColor = Color.Red,
            PromptColor = Color.Magenta,
            OkColor = Color.Green,
            TitleColor = Color.YellowGreen,
            InformationColor = Color.DarkCyan,
            SystemColor = Color.DarkBlue,
            WarningColor=Color.Orange,
            BackgroundColor=Color.Black
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
        static Color SelectColor(IColorSetting colorSetting, OutputType type = OutputType.Default,
            Color? customColor = null)
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