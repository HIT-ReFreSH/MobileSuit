using System;

namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    /// Color settings of a IOServer.
    /// </summary>
    public struct IOServerColorSetting : IEquatable<IOServerColorSetting>
    {
        /// <summary>
        /// Default color. For OutputType.Default
        /// </summary>
        public ConsoleColor DefaultColor { get; set; }
        /// <summary>
        /// Prompt Color. For OutputType.Prompt
        /// </summary>
        public ConsoleColor PromptColor { get; set; }
        /// <summary>
        /// Prompt Color. For OutputType.Error
        /// </summary>
        public ConsoleColor ErrorColor { get; set; }
        /// <summary>
        /// Prompt Color. For OutputType.AllOK
        /// </summary>
        public ConsoleColor AllOkColor { get; set; }
        /// <summary>
        /// Prompt Color. For OutputType.ListTitle
        /// </summary>
        public ConsoleColor ListTitleColor { get; set; }
        /// <summary>
        /// Prompt Color. For OutputType.CustomInformation
        /// </summary>
        public ConsoleColor CustomInformationColor { get; set; }
        /// <summary>
        /// Prompt Color. For OutputType.Information
        /// </summary>
        public ConsoleColor InformationColor { get; set; }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="other">The object to compare with the current instance.</param>
        /// <returns>whether this instance and a specified object are equal.</returns>
        public bool Equals(IOServerColorSetting other)
        {
            return DefaultColor == other.DefaultColor && PromptColor == other.PromptColor && ErrorColor == other.ErrorColor && AllOkColor == other.AllOkColor && ListTitleColor == other.ListTitleColor && CustomInformationColor == other.CustomInformationColor && InformationColor == other.InformationColor;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="that">The object to compare with the current instance.</param>
        /// <returns>whether this instance and a specified object are equal.</returns>
        public override bool Equals(object that)
        {
            return that is IOServerColorSetting && Equals((IOServerColorSetting)that);


        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine((int)DefaultColor, (int)PromptColor, (int)ErrorColor, (int)AllOkColor, (int)ListTitleColor, (int)CustomInformationColor, (int)InformationColor);
        }

        /// <summary>
        /// Indicates whether two instances are equal.
        /// </summary>
        /// <param name="left">left instance</param>
        /// <param name="right">right instance</param>
        /// <returns>true for equal</returns>
        public static bool operator ==(IOServerColorSetting left, IOServerColorSetting right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// Indicates whether two instances are not-equal.
        /// </summary>
        /// <param name="left">left instance</param>
        /// <param name="right">right instance</param>
        /// <returns>true for not-equal</returns>
        public static bool operator !=(IOServerColorSetting left, IOServerColorSetting right)
        {
            return !(left == right);
        }

    }
}
