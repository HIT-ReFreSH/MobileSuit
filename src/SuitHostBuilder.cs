using PlasticMetal.MobileSuit.Core;
using System;
using System.Collections.Generic;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel;
using System.IO;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// Builder to build a MobileSuit host.
    /// </summary>
    public class SuitHostBuilder
    {
        internal SuitHostBuilder(){}
        /// <summary>
        /// ColorSetting of host's IO
        /// </summary>
        protected internal ColorSetting? ColorSetting { get; set; }
        /// <summary>
        /// Use given color setting for host
        /// </summary>
        /// <param name="setting">given color setting</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureColor(ColorSetting setting)
        {
            ColorSetting = setting;
            return this;
        }
        /// <summary>
        /// Input Stream of host's IO
        /// </summary>
        protected internal TextReader? Input { get; set; }
        /// <summary>
        /// Use given stream as input for host
        /// </summary>
        /// <param name="stream">given stream</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureIn(TextReader stream)
        {
            Input = stream;
            return this;
        }
        /// <summary>
        /// Output Stream of host's IO
        /// </summary>
        protected internal TextWriter? Output { get; set; }
        /// <summary>
        /// Use given stream as output for host
        /// </summary>
        /// <param name="stream">given stream</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureOut(TextWriter stream)
        {
            Output = stream;
            return this;
        }
        /// <summary>
        /// Error Stream of host's IO
        /// </summary>
        protected internal TextWriter? Error { get; set; }
        /// <summary>
        /// Use given stream as error output for host
        /// </summary>
        /// <param name="stream">given stream</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureError(TextWriter stream)
        {
            Error = stream;
            return this;
        }
        /// <summary>
        /// Settings of host
        /// </summary>
        protected internal HostSettings Settings { get; set; } = new HostSettings();

        /// <summary>
        /// Use given settings for host
        /// </summary>
        /// <param name="settings">given settings</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureSetting(HostSettings settings)
        {
            Settings = settings;
            return this;
        }

        /// <summary>
        /// Logger of host
        /// </summary>
        protected internal Logger? Logger { get; set; }

        /// <summary>
        /// Use given logger for host
        /// </summary>
        /// <param name="logger">given logger</param>
        /// <returns>this</returns>
        public SuitHostBuilder UseLog(Logger logger)
        {
            Logger = logger;
            return this;
        }

        /// <summary>
        /// BuildInCommandServer of host
        /// </summary>
        protected internal Type BuildInCommandServer { get; set; } = typeof(BuildInCommandServer);

        /// <summary>
        /// IOServer of host
        /// </summary>
        protected internal Type IOServer { get; set; } = typeof(IOServer);

        /// <summary>
        /// PromptServer of host
        /// </summary>
        protected internal Type PromptServer { get; set; } = typeof(PromptServer);
        /// <summary>
        /// Get the host under the configuration of the builder
        /// </summary>
        /// <param name="instance">instance to drive</param>
        /// <returns>The host under the configuration of the builder</returns>
        public IMobileSuitHost Build(object instance)
        {
            var io =
                IOServer.GetConstructor(types: Array.Empty<Type>())?.Invoke(null) as IIOServer
                     ?? Suit.GeneralIO;
            if (Input != null)
            {
                io.Input = Input;
            }

            if (Output != null) io.Output = Output;
            if (ColorSetting != null) io.ColorSetting = ColorSetting;
            if (Error != null) io.ErrorStream = Error;
            Logger ??= ILogger.OfTemp();
            var prompt =
                PromptServer.GetConstructor(Array.Empty<Type>())?.Invoke(null) as IPromptServer
                ?? IPromptServer.DefaultPromptServer;
            prompt.IO.Assign(io);
            io.Prompt = prompt;
            return new SuitHost(instance, Logger, io, BuildInCommandServer, prompt);

        }
    }
    /// <summary>
    /// Extensions for MobileSuitHostBuilder
    /// </summary>
    public static class SuitHostBuilderExtensions
    {
        /// <summary>
        /// Use given PromptServer for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <typeparam name="T">PromptServer</typeparam>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder UsePrompt<T>(this SuitHostBuilder builder)
        where T : IPromptServer
        {
            builder ??= new SuitHostBuilder();
            builder.PromptServer = typeof(T);
            return builder;
        }
        /// <summary>
        /// Use given IOServer for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <typeparam name="T">IOServer</typeparam>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder UseIO<T>(this SuitHostBuilder builder)
            where T : IIOServer
        {
            builder ??= new SuitHostBuilder();
            builder.IOServer = typeof(T);
            return builder;
        }
        /// <summary>
        /// Use given BuildInCommandServer for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <typeparam name="T">BuildInCommandServer</typeparam>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder UseBuildInCommand<T>(this SuitHostBuilder builder)
        where T : IBuildInCommandServer
        {
            builder ??= new SuitHostBuilder();
            builder.BuildInCommandServer = typeof(T);
            return builder;
        }


        /// <summary>
        /// Get the host under the configuration of the builder
        /// </summary>
        /// <param name="builder">the builder</param>
        /// <typeparam name="T">Type to drive</typeparam>
        /// <returns></returns>
        public static IMobileSuitHost Build<T>(this SuitHostBuilder builder)
        where T:new()
            => builder?.Build(typeof(T).GetConstructor(Array.Empty<Type>())?.Invoke(null) ?? new object())
               ??new SuitHost(new object(),ILogger.OfTemp(),Suit.GeneralIO,typeof(BuildInCommandServer),IPromptServer.DefaultPromptServer);
    }


}
