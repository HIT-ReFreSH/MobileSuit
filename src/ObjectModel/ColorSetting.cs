using System;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     Color settings of a Mobile Suit.
    /// </summary>
    public struct ColorSetting : IColorSetting, IEquatable<ColorSetting>
    {
        /// <inheritdoc/>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        ///     Default color. For OutputType.Default
        /// </summary>
        public ConsoleColor DefaultColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Prompt
        /// </summary>
        public ConsoleColor PromptColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Error
        /// </summary>
        public ConsoleColor ErrorColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.AllOK
        /// </summary>
        public ConsoleColor OkColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.ListTitle
        /// </summary>
        public ConsoleColor TitleColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.CustomInformation
        /// </summary>
        public ConsoleColor InformationColor { get; set; }

        /// <summary>
        ///     Prompt Color. For OutputType.Information
        /// </summary>
        public ConsoleColor SystemColor { get; set; }
        /// <inheritdoc/>
        public ConsoleColor WarningColor { get; set; }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="other">The object to compare with the current instance.</param>
        /// <returns>whether this instance and a specified object are equal.</returns>
        public bool Equals(IColorSetting? other)
        {
            return other != null &&
                   DefaultColor == other.DefaultColor && PromptColor == other.PromptColor &&
                   ErrorColor == other.ErrorColor && OkColor == other.OkColor &&
                   TitleColor == other.TitleColor && InformationColor == other.InformationColor &&
                   SystemColor == other.SystemColor && WarningColor==other.WarningColor &&
                   BackgroundColor==other.BackgroundColor;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="that">The object to compare with the current instance.</param>
        /// <returns>whether this instance and a specified object are equal.</returns>
        public override bool Equals(object? that)
        {
            return that is IColorSetting && Equals((ColorSetting) that);
        }

        /// <summary>
        ///     generate hash code of all colors
        /// </summary>
        /// <returns> hash code of all colors</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(HashCode.Combine((int) DefaultColor, (int) PromptColor, (int) ErrorColor, (int) OkColor,
                (int) TitleColor, (int) InformationColor, (int) SystemColor, (int)WarningColor), (int)BackgroundColor);
        }

        /// <summary>
        ///     Indicates whether two instances are equal.
        /// </summary>
        /// <param name="left">left instance</param>
        /// <param name="right">right instance</param>
        /// <returns>true for equal</returns>
        public static bool operator ==(ColorSetting left, ColorSetting right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Indicates whether two instances are not-equal.
        /// </summary>
        /// <param name="left">left instance</param>
        /// <param name="right">right instance</param>
        /// <returns>true for not-equal</returns>
        public static bool operator !=(ColorSetting left, ColorSetting right)
        {
            return !(left == right);
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="other">The object to compare with the current instance.</param>
        /// <returns>whether this instance and a specified object are equal.</returns>
        public bool Equals(ColorSetting other)
        {
            return Equals(other as IColorSetting);
        }
    }
}