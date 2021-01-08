#nullable enable
using System;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    ///     A entity, which serves the input/output of a mobile suit.
    /// </summary>
    public partial class IOHub : IIOHub
    {
        private PromptGenerator? _prompt;


        /// <summary>
        ///     Initialize a IOServer.
        /// </summary>
        public IOHub()
        {
            ColorSetting = IColorSetting.DefaultColorSetting;
            Input = Console.In;
            Output = Console.Out;
            ErrorStream = Console.Error;
        }

        ///// <summary>
        /////     Initialize a IOServer.
        ///// </summary>
        //public IOServer(ISuitConfiguration configuration)
        //{
        //    ColorSetting = configuration?.ColorSetting ?? IColorSetting.DefaultColorSetting;
        //    Prompt = configuration?.PromptServer ?? IPromptServer.DefaultPromptServer;
        //    Input = Console.In;
        //    Output = Console.Out;
        //    ErrorStream = Console.Error;
        //}

        /// <summary>
        ///     Color settings for this IOServer. (default DefaultColorSetting)
        /// </summary>
        public IColorSetting ColorSetting { get; set; }

        /// <summary>
        ///     Prompt server for the io server.
        /// </summary>
        public PromptGenerator Prompt
        {
            get => _prompt ??= PromptGenerator.DefaultPromptServer;
            set => _prompt = value;
        }
    }
}