using System;
using System.IO;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Logging;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.UI;
using PlasticMetal.MobileSuit.UI.PowerLine;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Builder to build a MobileSuit host.
    /// </summary>
    public class SuitHostBuilder
    {
        internal SuitHostBuilder()
        {
            this.UseBasicPrompt();
        }

        /// <summary>
        ///     ColorSetting of host's IO
        /// </summary>
        protected internal ColorSetting? ColorSetting { get; set; }

        /// <summary>
        ///     Input Stream of host's IO
        /// </summary>
        protected internal TextReader? Input { get; set; }

        /// <summary>
        ///     Output Stream of host's IO
        /// </summary>
        protected internal TextWriter? Output { get; set; }

        /// <summary>
        ///     Error Stream of host's IO
        /// </summary>
        protected internal TextWriter? Error { get; set; }

        /// <summary>
        ///     Settings of host
        /// </summary>
        protected internal HostSettings Settings { get; set; }

        /// <summary>
        ///     Logger of host
        /// </summary>
        protected internal ISuitLogger? Logger { get; set; }

        /// <summary>
        ///     BuildInCommandServer of host
        /// </summary>
        protected internal Type BuildInCommandServer { get; set; } = typeof(BuildInCommandServer);

        /// <summary>
        ///     IOServer of host
        /// </summary>
        protected internal Type IOServer { get; set; } = typeof(IOHub);

        /// <summary>
        ///     PromptServer of host
        /// </summary>
        protected internal PromptGeneratorBuilder PromptBuilder { get; set; } = new();

        /// <summary>
        ///     Use given color setting for host
        /// </summary>
        /// <param name="setting">given color setting</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureColor(ColorSetting setting)
        {
            ColorSetting = setting;
            return this;
        }

        /// <summary>
        ///     Use given stream as input for host
        /// </summary>
        /// <param name="stream">given stream</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureIn(TextReader stream)
        {
            Input = stream;
            return this;
        }

        /// <summary>
        ///     Use given stream as output for host
        /// </summary>
        /// <param name="stream">given stream</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureOut(TextWriter stream)
        {
            Output = stream;
            return this;
        }

        /// <summary>
        ///     Use given stream as error output for host
        /// </summary>
        /// <param name="stream">given stream</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureError(TextWriter stream)
        {
            Error = stream;
            return this;
        }

        /// <summary>
        ///     Use given settings for host
        /// </summary>
        /// <param name="settings">given settings</param>
        /// <returns>this</returns>
        public SuitHostBuilder ConfigureSetting(HostSettings settings)
        {
            Settings = settings;
            return this;
        }

        /// <summary>
        ///     Use given logger for host
        /// </summary>
        /// <param name="logger">given logger</param>
        /// <returns>this</returns>
        public SuitHostBuilder UseLog(ISuitLogger logger)
        {
            Logger = logger;
            return this;
        }

        /// <summary>
        ///     Get the host under the configuration of the builder
        /// </summary>
        /// <param name="instance">instance to drive</param>
        /// <returns>The host under the configuration of the builder</returns>
        public IMobileSuitHost Build(object instance)
        {
            var io =
                IOServer.GetConstructor(Array.Empty<Type>())?.Invoke(null) as IIOHub
                ?? Suit.GeneralIO;
            if (Input != null) io.Input = Input;

            if (Output != null) io.Output = Output;
            if (ColorSetting != null) io.ColorSetting = ColorSetting;
            if (Error != null) io.ErrorStream = Error;
            Logger ??= ISuitLogger.CreateEmpty();

            var host = new SuitHost(instance, Logger, io, BuildInCommandServer);
            var prompt = PromptBuilder.Build(host, io, instance);
            host.Prompt.Assign(prompt);
            io.Prompt.Assign(prompt);
            return host;
        }
    }

    /// <summary>
    ///     Extensions for MobileSuitHostBuilder
    /// </summary>
    public static class SuitHostBuilderExtensions
    {
        /// <summary>
        ///     Use given PromptGenerator for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <param name="options">Options for the generator.</param>
        /// <typeparam name="T">PromptServer</typeparam>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder UsePrompt<T>(this SuitHostBuilder builder,
            Action<PromptGeneratorBuilder>? options = null)
            where T : IPromptGenerator
        {
            builder.PromptBuilder = new PromptGeneratorBuilder { GeneratorType = typeof(T) };
            options?.Invoke(builder.PromptBuilder);
            return builder;
        }
        /// <summary>
        ///     Use BasicPromptGenerator for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <param name="options">Options for the generator.</param>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder UseBasicPrompt(this SuitHostBuilder builder,
            Action<PromptGeneratorBuilder>? options = null)
        {
            builder.PromptBuilder.GeneratorType = typeof(BasicPromptGenerator);
            builder.PromptBuilder.UseInformation();
            builder.PromptBuilder.AddProvider<BasicPromptGenerator.InputExpressionPromptProvider>();
            builder.PromptBuilder.AddProvider<BasicPromptGenerator.InputDefaultValuePromptProvider>();
            options?.Invoke(builder.PromptBuilder);
            builder.PromptBuilder.UseTraceBack();
            builder.PromptBuilder.UseReturnValue();
            return builder;
        }

        /// <summary>
        ///     Use PowerLinePromptGenerator for the Host. 
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <param name="prefixCross">Use prefix cross or suffix label for TraceBack.</param>
        /// <param name="options">Options for the generator.</param>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder UsePowerLine(this SuitHostBuilder builder, bool prefixCross=true,
            Action<PromptGeneratorBuilder>? options = null)
        {
            builder.PromptBuilder = new();
            builder.PromptBuilder.GeneratorType = typeof(PowerLineGenerator);
            if(prefixCross) builder.PromptBuilder.UseCross();
            builder.PromptBuilder.UseInformation();
            builder.PromptBuilder.AddProvider<PowerLineGenerator.InputExpressionPromptProvider>();
            builder.PromptBuilder.AddProvider<PowerLineGenerator.InputDefaultValuePromptProvider>();
            options?.Invoke(builder.PromptBuilder);
            if (!prefixCross) builder.PromptBuilder.UseTraceBack();
            builder.PromptBuilder.UseReturnValue();
            return builder;
        }
        /// <summary>
        /// Use given IOHub for the Host.
        /// </summary>
        /// <param name="builder">Builder of the Host</param>
        /// <typeparam name="T">Type of IOHub</typeparam>
        /// <returns>Builder of the Host</returns>
        public static SuitHostBuilder UseIO<T>(this SuitHostBuilder builder)
            where T : IIOHub
        {
            builder ??= new SuitHostBuilder();
            builder.IOServer = typeof(T);
            return builder;
        }

        /// <summary>
        ///     Use given BuildInCommandServer for the Host
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
        ///     Get the host under the configuration of the builder
        /// </summary>
        /// <param name="builder">the builder</param>
        /// <typeparam name="T">Type to drive</typeparam>
        /// <returns></returns>
        public static IMobileSuitHost Build<T>(this SuitHostBuilder builder)
            where T : new()
        {
            return builder?.Build(typeof(T).GetConstructor(Array.Empty<Type>())?.Invoke(null) ?? new object())
                   ?? new SuitHost(new object(), ISuitLogger.CreateEmpty(), Suit.GeneralIO,
                       typeof(BuildInCommandServer));
        }
    }
    /// <summary>
    ///     Extensions for PromptGeneratorBuilder
    /// </summary>
    public static class PromptGeneratorBuilderExtensions
    {
        /// <summary>
        ///     Add given PromptProvider for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <typeparam name="T">PromptProvider</typeparam>
        /// <returns>Builder for the host</returns>
        public static PromptGeneratorBuilder AddProvider<T>(this PromptGeneratorBuilder builder)
            where T : IPromptProvider
        {
            builder.AddProvider(typeof(T));
            return builder;
        }
        /// <summary>
        ///     Add TraceBackPromptProvider for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <returns>Builder for the host</returns>
        public static PromptGeneratorBuilder UseTraceBack(this PromptGeneratorBuilder builder)
        {
            builder.AddProvider<TraceBackPromptProvider>();
            return builder;
        }
        /// <summary>
        ///     Add CrossTraceBackProvider for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <returns>Builder for the host</returns>
        public static PromptGeneratorBuilder UseCross(this PromptGeneratorBuilder builder)
        {
            builder.AddProvider<CrossTraceBackPromptProvider>();
            return builder;
        }
        /// <summary>
        ///     Add InformationPromptProvider for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <returns>Builder for the host</returns>
        public static PromptGeneratorBuilder UseInformation(this PromptGeneratorBuilder builder)
        {
            builder.AddProvider<InformationPromptProvider>();
            return builder;
        }
        /// <summary>
        ///     Add ReturnValuePromptProvider for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <returns>Builder for the host</returns>
        public static PromptGeneratorBuilder UseReturnValue(this PromptGeneratorBuilder builder)
        {
            builder.AddProvider<ReturnValuePromptProvider>();
            return builder;
        }
        /// <summary>
        ///     Add given PromptProvider for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <param name="selector">Select the provider from source.</param>
        /// <typeparam name="TProvider">PromptProvider</typeparam>
        /// <typeparam name="TSource">Source for the provider.</typeparam>
        /// <returns>Builder for the host</returns>
        public static PromptGeneratorBuilder AddProvider<TProvider,TSource>(this PromptGeneratorBuilder builder,
            Func<TSource,TProvider> selector)
            where TProvider : IPromptProvider
        {
            builder.AddProvider(typeof(TProvider),selector);
            return builder;
        }
        
    }
}