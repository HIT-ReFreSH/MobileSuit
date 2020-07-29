using System;
using System.Globalization;
using System.Reflection;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.IO;

namespace PlasticMetal.MobileSuit
{
    /// <inheritdoc />
    public class SuitConfiguration : ISuitConfiguration
    {
        /// <summary>
        ///     Initialize a configuration
        /// </summary>
        /// <param name="buildInCommandServerType">type of builtInCommandServerType</param>
        /// <param name="io">io server</param>
        /// <param name="promptServer">prompt server</param>
        /// <param name="colorSetting">color setting </param>
        public SuitConfiguration(Type buildInCommandServerType, IIOServer io, IPromptServer promptServer,
            IColorSetting colorSetting)
        {
            BuildInCommandServerType = buildInCommandServerType;
            IO = io;
            PromptServer = promptServer;
            ColorSetting = colorSetting;
        }

        /// <inheritdoc />
        public Type BuildInCommandServerType { get; }

        /// <inheritdoc />
        public void InitializeBuildInCommandServer(SuitHost host)
        {
            BuildInCommandServer = BuildInCommandServerType.Assembly.CreateInstance(
                BuildInCommandServerType.FullName ?? BuildInCommandServerType.Name, true,
                BindingFlags.Default, null,
                new object[] {host}, CultureInfo.CurrentCulture, null) as IBuildInCommandServer;
        }

        /// <inheritdoc />
        public IIOServer IO { get; }

        /// <inheritdoc />
        public IBuildInCommandServer? BuildInCommandServer { get; protected set; }

        /// <inheritdoc />
        public IPromptServer PromptServer { get; }

        /// <inheritdoc />
        public IColorSetting ColorSetting { get; }
    }
}