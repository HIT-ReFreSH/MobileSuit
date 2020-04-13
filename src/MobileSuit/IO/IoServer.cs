#nullable enable
using System;

namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    /// Type of content that writes to the output stream.
    /// </summary>
    public enum OutputType
    {
        /// <summary>
        /// Normal content.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Prompt content.
        /// </summary>
        Prompt = 1,
        /// <summary>
        /// Error content.
        /// </summary>
        Error = 2,
        /// <summary>
        /// All-Ok content.
        /// </summary>
        AllOk = 3,
        /// <summary>
        /// Title of a list.
        /// </summary>
        ListTitle = 4,
        /// <summary>
        /// Normal information.
        /// </summary>
        CustomInfo = 5,
        /// <summary>
        /// Information provided by MobileSuit.
        /// </summary>
        MobileSuitInfo = 6
    }
    /// <summary>
    /// A entity, which serves the input/output of a mobile suit.
    /// </summary>
    public partial class IOServer
    {
        /// <summary>
        /// Default color settings for IOServer.
        /// </summary>
        public static IOServerColorSetting DefaultColorSetting { get; } = new IOServerColorSetting
        {
            DefaultColor = ConsoleColor.White,
            ErrorColor = ConsoleColor.Red,
            PromptColor = ConsoleColor.Magenta,
            AllOkColor = ConsoleColor.Green,
            ListTitleColor = ConsoleColor.Yellow,
            CustomInformationColor = ConsoleColor.DarkCyan,
            InformationColor = ConsoleColor.DarkBlue
        };
        /// <summary>
        /// Color settings for this IOServer. (default DefaultColorSetting)
        /// </summary>
        public IOServerColorSetting ColorSetting { get; set; }

        /// <summary>
        /// Initialize a IOServer.
        /// </summary>
        public IOServer()
        {
            ColorSetting = DefaultColorSetting;
            Input = Console.In;
            Output = Console.Out;
            Error = Console.Error;
            
        }
        /// <summary>
        /// Console's Title. Linked to System.Console.Title.
        /// </summary>
        public string ConsoleTitle
        {
            get => Console.Title;
            set => Console.Title = value;
        }
        /// <summary>
        /// Color settings of a IOServer.
        /// </summary>
        public struct IOServerColorSetting
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
            /// <returns>
            /// <see langword="true" /> if <paramref name="other" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
            public bool Equals(IOServerColorSetting other)
            {
                return DefaultColor == other.DefaultColor && PromptColor == other.PromptColor && ErrorColor == other.ErrorColor && AllOkColor == other.AllOkColor && ListTitleColor == other.ListTitleColor && CustomInformationColor == other.CustomInformationColor && InformationColor == other.InformationColor;
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return HashCode.Combine((int) DefaultColor, (int) PromptColor, (int) ErrorColor, (int) AllOkColor, (int) ListTitleColor, (int) CustomInformationColor, (int) InformationColor);
            }

            public static bool operator ==(IOServerColorSetting left, IOServerColorSetting right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(IOServerColorSetting left, IOServerColorSetting right)
            {
                return !(left == right);
            }
        }
    }
}