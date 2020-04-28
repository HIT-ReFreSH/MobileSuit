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

    }
}