using System;
using PlasticMetal.MobileSuit.IO;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// represent a configuration for Mobile Suit
    /// </summary>
    public interface ISuitConfiguration
    {
        /// <summary>
        /// Type of BuiltInCommandServer
        /// </summary>
        Type BuiltInCommandServerType { get; }

        /// <summary>
        /// Initialize the BuiltInCommandServer with BuiltInCommandServerType and given host
        /// </summary>
        /// <param name="host">host for BuiltInCommandServer</param>
        void InitializeBuiltInCommandServer(SuitHost host);

        /// <summary>
        /// IOServer for the Mobile Suit
        /// </summary>
        IIOServer IO { get;}
        /// <summary>
        /// BuildInCommandServer for the Mobile Suit
        /// </summary>
        IBuiltInCommandServer? BuiltInCommandServer { get; }
        /// <summary>
        /// PromptServer for the Mobile Suit
        /// </summary>
        IPromptServer PromptServer { get; }
        /// <summary>
        /// ColorSetting for the Mobile Suit
        /// </summary>
        IColorSetting ColorSetting { get; }

        /// <summary>
        /// get a default configuration of Mobile Suit
        /// </summary>
        /// <returns></returns>
        static ISuitConfiguration GetDefaultConfiguration()
            => new SuitConfiguration(typeof(BuiltInCommandServer), 
                new IOServer(),
                new PromptServer(),
                IColorSetting.DefaultColorSetting);
    }
}
