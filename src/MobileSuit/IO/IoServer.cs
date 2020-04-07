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
    public partial class IoServer
    {
        /// <summary>
        /// Default color settings for IoServer.
        /// </summary>
        public static IoServerColorSetting DefaultColorSetting { get; } = new IoServerColorSetting
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
        /// Color settings for this IoServer. (default DefaultColorSetting)
        /// </summary>
        public IoServerColorSetting ColorSetting { get; set; }

        /// <summary>
        /// Initialize a IoServer.
        /// </summary>
        public IoServer()
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
        /// Color settings of a IoServer.
        /// </summary>
        public struct IoServerColorSetting
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
        }
    }
}