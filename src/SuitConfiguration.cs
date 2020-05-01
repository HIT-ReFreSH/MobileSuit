using System;
using System.Globalization;
using System.Reflection;
using PlasticMetal.MobileSuit.IO;

namespace PlasticMetal.MobileSuit
{
    /// <inheritdoc/>
    public class SuitConfiguration : ISuitConfiguration
    {
        /// <summary>
        /// Initialize a configuration
        /// </summary>
        /// <param name="builtInCommandServerType">type of builtInCommandServerType</param>
        /// <param name="io">io server</param>
        /// <param name="promptServer">prompt server</param>
        /// <param name="colorSetting">color setting </param>
        public SuitConfiguration(Type builtInCommandServerType, IIOServer io, IPromptServer promptServer, IColorSetting colorSetting)
        {
            BuiltInCommandServerType = builtInCommandServerType;
            IO = io;
            PromptServer = promptServer;
            ColorSetting = colorSetting;
        }

        /// <inheritdoc/>
        public Type BuiltInCommandServerType { get; }
        /// <inheritdoc/>
        public void InitializeBuiltInCommandServer(SuitHost host)
        {
            BuiltInCommandServer = BuiltInCommandServerType.Assembly.CreateInstance(
                BuiltInCommandServerType.FullName ?? BuiltInCommandServerType.Name, true,
                BindingFlags.Default, null,
                new object[] { host }, CultureInfo.CurrentCulture, null) as IBuiltInCommandServer;
        }
        /// <inheritdoc/>
        public IIOServer IO { get; }
        /// <inheritdoc/>
        public IBuiltInCommandServer? BuiltInCommandServer { get; protected set; }
        /// <inheritdoc/>
        public IPromptServer PromptServer { get; }
        /// <inheritdoc/>
        public IColorSetting ColorSetting { get; }
    }
}
