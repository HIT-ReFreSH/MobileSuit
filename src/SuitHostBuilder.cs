using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Core.Middleware;
using PlasticMetal.MobileSuit.Core.Services;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// Describes the work flow of mobile suit.
    /// </summary>
    public interface ISuitWorkFlow
    {
        /// <summary>
        /// Add a custom middleware
        /// </summary>
        /// <param name="middlewareType"></param>
        public ISuitWorkFlow UseCustom(Type middlewareType);
        /// <summary>
        /// Add suit prompt middleware
        /// </summary>
        public ISuitWorkFlow UsePrompt();
        /// <summary>
        /// Add input middleware
        /// </summary>
        public ISuitWorkFlow UseInput();
        /// <summary>
        /// Add AppShell middleware
        /// </summary>
        public ISuitWorkFlow UseAppShell();
        /// <summary>
        /// Add HostShell middleware
        /// </summary>
        public ISuitWorkFlow UseHostShell();
        /// <summary>
        /// Add Finalize middleware
        /// </summary>
        public ISuitWorkFlow UseFinalize();
    }

    internal class SuitWorkFlow : ISuitWorkFlow
    {
        private List<Type> _middlewares = new();
        public ISuitWorkFlow UseCustom(Type middlewareType)
        {
            if (middlewareType.GetInterface(nameof(ISuitMiddleware)) is not null)
            {
                _middlewares.Add(middlewareType);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(middlewareType));
            }
            return this;
        }

        public ISuitWorkFlow UsePrompt()
            => this.UseCustom<PromptMiddleware>();

        public ISuitWorkFlow UseInput()
            => this.UseCustom<UserInputMiddleware>();

        public ISuitWorkFlow UseAppShell()
            => this.UseCustom<AppShellMiddleware>();

        public ISuitWorkFlow UseHostShell()
            => this.UseCustom<HostShellMiddleware>();

        public ISuitWorkFlow UseFinalize()
            => this.UseCustom<FinalizeMiddleware>();

        public IReadOnlyList<ISuitMiddleware> Build(IServiceProvider serviceProvider)
        {
            if (_middlewares.Count == 0)
            {
                UsePrompt()
                    .UseInput()
                    .UseHostShell()
                    .UseAppShell()
                    .UseFinalize();
            }
            var r = new List<ISuitMiddleware>();
            foreach (var type in _middlewares)
            {
                if (ActivatorUtilities.CreateInstance(serviceProvider, type) is ISuitMiddleware middleware)
                {
                    r.Add(middleware);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(type.FullName);
                }
            }
            return r;
        }
    }
    /// <summary>
    ///     Builder to build a MobileSuit host.
    /// </summary>
    public class SuitHostBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public SuitAppInfo AppInfo { get; } = new();
        private List<SuitShell> _clients = new();
        private Type _commandServer = typeof(SuitCommandServer);
        private readonly SuitWorkFlow _workFlow = new();
        /// <summary>
        /// 
        /// </summary>
        public ISuitWorkFlow WorkFlow => _workFlow;
        /// <summary>
        /// Service collection of suit host.
        /// </summary>
        public IServiceCollection Services { get; } = new ServiceCollection();
        /// <summary>
        /// Service collection of suit host.
        /// </summary>
        public ConfigurationManager Configuration { get; } = new();
        /// <summary>
        /// Add a client shell to mobile suit
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(SuitShell client)
           => _clients.Add(client);

        /// <summary>
        /// Add a client shell to mobile suit
        /// </summary>
        /// <param name="serverType"></param>
        public void UseCommandServer(Type serverType)
        {
            if (serverType.GetInterface(nameof(ISuitCommandServer)) is null)
            {
                throw new ArgumentOutOfRangeException(nameof(serverType));
            }

            Services.Add(new(typeof(ISuitCommandServer), serverType, ServiceLifetime.Scoped));
            _commandServer = serverType;
        }
        /// <summary>
        /// config IO
        /// </summary>
        /// <param name="configurer"></param>
        public void ConfigureIO(IIOHubConfigurer configurer)
        {
            Services.AddSingleton(configurer);
        }

        /// <summary>
        /// Build a SuitHost.
        /// </summary>
        /// <returns></returns>
        public IHost Build()
        {
            Services.AddSingleton<ISuitAppInfo>(AppInfo);
            Services.AddSingleton(SuitAppShell.FromClients(_clients));
            Services.AddSingleton(SuitHostShell.FromCommandServer(_commandServer));
            var providers = Services.BuildServiceProvider();
            return new SuitHost(providers, _workFlow.Build(providers));
        }

        internal SuitHostBuilder(string[]? args)
        {

            AppInfo.StartArgs = args ?? Array.Empty<string>();
            Services.AddScoped<ISuitCommandServer, SuitCommandServer>();
            Services.AddSingleton<PromptFormatter>(PromptFormatters.BasicPromptFormatter);
            Services.AddSingleton<TaskService>();
            Services.AddSingleton<HistoryService>();
            Services.AddSingleton<IParsingService, ParsingService>();
            Services.AddSingleton<ISuitExceptionHandler, SuitExceptionHandler>();
        }
    }

    /// <summary>
    ///     Extensions for MobileSuitHostBuilder
    /// </summary>
    public static class SuitHostBuilderExtensions
    {
        /// <summary>
        /// Add a custom middleware
        /// </summary>
        public static ISuitWorkFlow UseCustom<T>(this ISuitWorkFlow workFlow)
        where T : ISuitMiddleware
        {
            return workFlow.UseCustom(typeof(T));
        }
        /// <summary>
        ///     Use given PromptGenerator for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <typeparam name="T">PromptServer</typeparam>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder AddObject<T>(this SuitHostBuilder builder)
        {
            builder.AddClient(SuitObjectShell.FromType(typeof(T)));
            return builder;
        }

        /// <summary>
        ///     Use given PromptGenerator for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <param name="instance"></param>
        /// <typeparam name="T">PromptServer</typeparam>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder AddObject<T>(this SuitHostBuilder builder, T instance)
        {
            builder.AddClient(SuitObjectShell.FromInstance(typeof(T), _ => instance));
            return builder;
        }
        /// <summary>
        ///     Use given PromptGenerator for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <param name="name"></param>
        /// <param name="method"></param>
        /// <typeparam name="T">PromptServer</typeparam>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder AddMethod<T>(this SuitHostBuilder builder, string name,Delegate method)
        {
            builder.AddClient(SuitMethodShell.FromDelegate(name,method));
            return builder;
        }
        /// <summary>
        ///     Use given PromptGenerator for the Host
        /// </summary>
        /// <param name="builder">Builder for the host</param>
        /// <returns>Builder for the host</returns>
        public static SuitHostBuilder UsePowerLine(this SuitHostBuilder builder)
        {
            builder.Services.AddSingleton<PromptFormatter>(PromptFormatters.BasicPromptFormatter);
            return builder;
        }
    }
}