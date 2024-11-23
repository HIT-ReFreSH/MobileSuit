using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Builder to build a MobileSuit host.
/// </summary>
public class SuitHostBuilder
{
    private readonly TaskRecorder _cancelTasks = new();
    /// <summary>
    /// Object SuitShell Clients
    /// </summary>
    protected readonly List<SuitShell> Clients = new();
    protected Type CommandServer = typeof(SuitCommandServer);

    /// <summary>
    ///
    /// </summary>
    /// <param name="args"></param>
    internal SuitHostBuilder(string[]? args)
    {
        AppInfo.StartArgs = args ?? [];
        Configuration.AddEnvironmentVariables();

        Services.AddSingleton<PromptFormatter>(PromptFormatters.BasicPromptFormatter);
        Services.AddSingleton<ITaskService>(new TaskService(_cancelTasks));
        Services.AddSingleton<IHistoryService, HistoryService>();
        Services.AddScoped<ISuitCommandServer, SuitCommandServer>();
        Services.AddScoped<ISuitContextProperties, SuitContextProperties>();
        Services.AddScoped<ISuitContextProperties, SuitContextProperties>();
        Services.AddScoped<IIOHubYouShouldNeverUse, FourBitIOHub>();
        Services.AddScoped<IIOHub, RealIOHub>();
        Services.AddScoped<ISuitLogBucket, SuitLogBucket>();
        Services.AddLogging();
        Services.AddSingleton<IIOHubConfigurator>(_ => { });
        Services.AddSingleton(Parsing);
        Services.AddSingleton<ISuitExceptionHandler, SuitExceptionHandler>();
    }
    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="origin"></param>
    protected internal SuitHostBuilder(SuitHostBuilder origin)
    {
        AppInfo = origin.AppInfo;
        Configuration = origin.Configuration;
        Services=origin.Services;
        Clients=origin.Clients;
        CommandServer = origin.CommandServer;
        Parsing = origin.Parsing;
        WorkFlow = origin.WorkFlow;
    }
    /// <summary>
    /// </summary>
    public SuitAppInfo AppInfo { get; } = new();

    /// <summary>
    /// </summary>
    public IParsingService Parsing { get; set; } = new ParsingService();

    /// <summary>
    /// </summary>
    public ISuitWorkFlow WorkFlow { get; } = new SuitWorkFlow();

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
    public void AddClient(SuitShell client) { Clients.Add(client); }

    /// <summary>
    ///     Add a client shell to mobile suit
    /// </summary>
    /// <param name="serverType"></param>
    public void UseCommandServer(Type serverType)
    {
        if (serverType.GetInterface(nameof(ISuitCommandServer)) is null)
            throw new ArgumentOutOfRangeException(nameof(serverType));

        Services.Add(new ServiceDescriptor(typeof(ISuitCommandServer), serverType, ServiceLifetime.Scoped));
        CommandServer = serverType;
    }

    /// <summary>
    ///     config IO
    /// </summary>
    /// <param name="configurer"></param>
    public void ConfigureIO(IIOHubConfigurator configurer) { Services.AddSingleton(configurer); }

    /// <summary>
    ///     Build a SuitHost.
    /// </summary>
    /// <returns></returns>
    public virtual IMobileSuitHost Build()
    {
        AddPreBuildMatters();
        var startUp = new TaskCompletionSource();
        Services.AddSingleton<IHostApplicationLifetime>
        (
            new SuitHostApplicationLifetime
            (
                startUp,
                () =>
                {
                    _cancelTasks.IsLocked = true;
                    return Task.WhenAll(_cancelTasks);
                }
            )
        );

        var providers = Services.BuildServiceProvider();
        return new SuitHost(providers, WorkFlow.Build(providers), startUp, _cancelTasks);
    }
    /// <summary>
    /// Inject IConfiguration, SuitAppShell, SuitHostShell and ISuitAppInfo into Services
    /// </summary>
    /// <returns></returns>
    public SuitHostBuilder AddPreBuildMatters()
    {
        Services.AddSingleton<ISuitAppInfo>(AppInfo);
        Services.AddSingleton(SuitAppShell.FromClients(Clients));
        Services.AddSingleton(SuitHostShell.FromCommandServer(CommandServer));
        Services.AddSingleton<IConfiguration>(Configuration);
        return this;
    }
}