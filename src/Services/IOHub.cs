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

        /// <summary>
        ///     Initialize a IOServer.
        /// </summary>
        public IOHub(IPromptFormatter promptGenerator, IIOHubConfigurer configurer)
        {
            ColorSetting = IColorSetting.DefaultColorSetting;
            Input = Console.In;
            Output=Console.Out;
            ErrorStream = Console.Error;
            Prompt = promptGenerator;
            configurer(this);
        }


        /// <summary>
        ///     Color settings for this IOServer. (default DefaultColorSetting)
        /// </summary>
        public IColorSetting ColorSetting { get; set; }


        /// <summary>
        ///     Prompt server for the io server.
        /// </summary>
        public IPromptFormatter Prompt{ get; }
    }
}