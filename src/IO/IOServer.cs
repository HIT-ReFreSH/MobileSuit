#nullable enable
using System;

namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    ///     A entity, which serves the input/output of a mobile suit.
    /// </summary>
    public partial class IOServer : IIOServer
    {
        private IPromptServer? _prompt;


        /// <summary>
        ///     Initialize a IOServer.
        /// </summary>
        public IOServer()
        {
            ColorSetting = IColorSetting.DefaultColorSetting;
            Input = Console.In;
            Output = Console.Out;
            ErrorStream = Console.Error;
        }

        /// <summary>
        ///     Initialize a IOServer.
        /// </summary>
        public IOServer(ISuitConfiguration configuration)
        {
            ColorSetting = configuration?.ColorSetting ?? IColorSetting.DefaultColorSetting;
            Prompt = configuration?.PromptServer ?? IPromptServer.DefaultPromptServer;
            Input = Console.In;
            Output = Console.Out;
            ErrorStream = Console.Error;
        }

        /// <summary>
        ///     Color settings for this IOServer. (default DefaultColorSetting)
        /// </summary>
        public IColorSetting ColorSetting { get; set; }

        /// <summary>
        ///     Prompt server for the io server.
        /// </summary>
        public IPromptServer Prompt
        {
            get => _prompt ??= IPromptServer.DefaultPromptServer;
            set => _prompt = value;
        }
    }
}