using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Services;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Builder to build a MobileSuit host.
/// </summary>
public class SuitHostBuilder
{
    private readonly List<SuitShell> _clients = new();
    private readonly SuitWorkFlow _workFlow = new();
    private Type _commandServer = typeof(SuitCommandServer);
    private TaskRecorder _cancelTasks = new();
    internal SuitHostBuilder(string[]? args)
    {
        AppInfo.StartArgs = args ?? Array.Empty<string>();
        Services.AddScoped<ISuitCommandServer, SuitCommandServer>();
        Services.AddSingleton<PromptFormatter>(PromptFormatters.BasicPromptFormatter);
        Services.AddSingleton<ITaskService>(new TaskService(_cancelTasks));
        Services.AddSingleton<IHistoryService, HistoryService>();
        Services.AddScoped<IIOHub, IOHub>();
        Services.AddLogging();
        Services.AddSingleton<IIOHubConfigurator>(_ => { });
        Services.AddSingleton(Parsing);
        Services.AddSingleton<ISuitExceptionHandler, SuitExceptionHandler>();
    }

    /// <summary>
    /// </summary>
    public SuitAppInfo AppInfo { get; } = new();

    /// <summary>
    /// </summary>
    public IParsingService Parsing { get; set; } = new ParsingService();

    /// <summary>
    /// </summary>
    public ISuitWorkFlow WorkFlow => _workFlow;

    /// <summary>
    ///     Service collection of suit host.
    /// </summary>
    public IServiceCollection Services { get; } = new ServiceCollection();

    /// <summary>
    ///     Service collection of suit host.
    /// </summary>
    public ConfigurationManager Configuration { get; } = new();

    /// <summary>
    ///     Add a client shell to mobile suit
    /// </summary>
    /// <param name="client"></param>
    public void AddClient(SuitShell client)
    {
        _clients.Add(client);
    }

    /// <summary>
    ///     Add a client shell to mobile suit
    /// </summary>
    /// <param name="serverType"></param>
    public void UseCommandServer(Type serverType)
    {
        if (serverType.GetInterface(nameof(ISuitCommandServer)) is null)
            throw new ArgumentOutOfRangeException(nameof(serverType));

        Services.Add(new ServiceDescriptor(typeof(ISuitCommandServer), serverType, ServiceLifetime.Scoped));
        _commandServer = serverType;
    }

    /// <summary>
    ///     config IO
    /// </summary>
    /// <param name="configurer"></param>
    public void ConfigureIO(IIOHubConfigurator configurer)
    {
        Services.AddSingleton(configurer);
    }

    /// <summary>
    ///     Build a SuitHost.
    /// </summary>
    /// <returns></returns>
    public IMobileSuitHost Build()
    {
        Services.AddSingleton<ISuitAppInfo>(AppInfo);
        Services.AddSingleton(SuitAppShell.FromClients(_clients));
        Services.AddSingleton(SuitHostShell.FromCommandServer(_commandServer));
        var startUp = new TaskCompletionSource();
        Services.AddSingleton<IHostApplicationLifetime>(
            new SuitHostApplicationLifetime(startUp, () =>
            {
                _cancelTasks.IsLocked = true;
                return Task.WhenAll(_cancelTasks);
            }));
        Services.AddSingleton<IConfiguration>(Configuration);
        var providers = Services.BuildServiceProvider();
        return new SuitHost(providers, _workFlow.Build(providers),startUp,_cancelTasks);
    }
}

/// <summary>
///     Extensions for MobileSuitHostBuilder
/// </summary>
public static class SuitHostBuilderExtensions
{
    /// <summary>
    ///     Add a custom middleware
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
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder)
    {
        builder.AddClient(SuitObjectShell.FromType(typeof(T)));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name">Set a name for the client</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder HasName(this SuitHostBuilder builder, string name)
    {
        builder.AppInfo.AppName = name;
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="instance"></param>
    /// <typeparam name="T">PromptServer</typeparam>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder, T instance)
    {
        builder.AddClient(SuitObjectShell.FromInstance(typeof(T), _ => instance));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name"></param>
    /// <typeparam name="T">PromptServer</typeparam>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder, string name)
    {
        builder.AddClient(SuitObjectShell.FromType(typeof(T), name));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name"></param>
    /// <param name="instance"></param>
    /// <typeparam name="T">PromptServer</typeparam>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder, string name, T instance)
    {
        builder.AddClient(SuitObjectShell.FromInstance(typeof(T), _ => instance, name));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name"></param>
    /// <param name="method"></param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder Map(this SuitHostBuilder builder, string name, Delegate method)
    {
        builder.AddClient(SuitMethodShell.FromDelegate(name, method));
        return builder;
    }

    /// <summary>
    ///     Use PowerLine PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder UsePowerLine(this SuitHostBuilder builder)
    {
        Console.OutputEncoding = Encoding.UTF8;
        builder.Services.AddSingleton<PromptFormatter>(PromptFormatters.PowerLineFormatter);
        return builder;
    }

    /// <summary>
    ///     Use Plain text IO for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder UsePureTextIO(this SuitHostBuilder builder)
    {
        builder.Services.AddScoped<IIOHub, PureTextIOHub>();
        return builder;
    }

    /// <summary>
    ///     Use 4-bit color IO for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder Use4BitColorIO(this SuitHostBuilder builder)
    {
        builder.Services.AddScoped<IIOHub, IOHub4Bit>();
        return builder;
    }

    ///// <summary>
    /////     Run a mobile suit
    ///// </summary>
    ///// <param name="host"></param>
    ///// <returns></returns>
    //public static async Task RunAsync(this IMobileSuitHost host)
    //{
    //    var token = new CancellationTokenSource().Token;
    //    await host.StartAsync(token).ConfigureAwait(false);
    //    await host.StopAsync(token).ConfigureAwait(false);
    //    host.Dispose();
    //}

    /// <summary>
    ///     Run a mobile suit
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static void Run(this IMobileSuitHost host)
    {
        host.RunAsync().GetAwaiter().GetResult();
    }
}