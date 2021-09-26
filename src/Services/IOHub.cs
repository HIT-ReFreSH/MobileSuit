#nullable enable
using System;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.Services;
using PlasticMetal.MobileSuit.UI;

namespace PlasticMetal.MobileSuit.Services
{
    
    /// <summary>
    ///     A entity, which serves the input/output of a mobile suit.
    /// </summary>
    public partial class IOHub : IIOHub
    {
        private PromptGenerator _promptGenerator;

        /// <summary>
        ///     Initialize a IOServer.
        /// </summary>
        public IOHub(IPromptGenerator promptGenerator, IOHubOptions options)
        {
            ColorSetting = options.ColorSetting;
            Input=options.Input;
            Output=options.Output;
            ErrorStream = options.Error;
            Prompt = promptGenerator;
        }


        /// <summary>
        ///     Color settings for this IOServer. (default DefaultColorSetting)
        /// </summary>
        public IColorSetting ColorSetting { get; set; }


        /// <summary>
        ///     Prompt server for the io server.
        /// </summary>
        public IPromptGenerator Prompt{ get; }
    }
}