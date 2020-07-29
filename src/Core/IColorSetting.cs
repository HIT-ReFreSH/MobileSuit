using System;

namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    ///     Color settings of a Mobile Suit.
    /// </summary>
    public interface IColorSetting : IEquatable<IColorSetting>
    {
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
        ///     Prompt Color. For OutputType.AllOK
        /// </summary>
        ConsoleColor AllOkColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.ListTitle
        /// </summary>
        ConsoleColor ListTitleColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.CustomInformation
        /// </summary>
        ConsoleColor CustomInformationColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Information
        /// </summary>
        ConsoleColor InformationColor { get; set; }

        /// <summary>
        ///     Default color settings for IOServer.
        /// </summary>
        public static ColorSetting DefaultColorSetting => new ColorSetting
        {
            DefaultColor = ConsoleColor.White,
            ErrorColor = ConsoleColor.Red,
            PromptColor = ConsoleColor.Magenta,
            AllOkColor = ConsoleColor.Green,
            ListTitleColor = ConsoleColor.DarkYellow,
            CustomInformationColor = ConsoleColor.DarkCyan,
            InformationColor = ConsoleColor.DarkBlue
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
            if (colorSetting == null) colorSetting = DefaultColorSetting;
            return
                customColor
                ?? type switch
                {
                    OutputType.Default => colorSetting.DefaultColor,
                    OutputType.Prompt => colorSetting.PromptColor,
                    OutputType.Error => colorSetting.ErrorColor,
                    OutputType.AllOk => colorSetting.AllOkColor,
                    OutputType.ListTitle => colorSetting.ListTitleColor,
                    OutputType.CustomInfo => colorSetting.CustomInformationColor,
                    OutputType.MobileSuitInfo => colorSetting.InformationColor,
                    _ => colorSetting.DefaultColor
                };
        }
    }
}