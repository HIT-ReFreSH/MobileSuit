using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Specifies the startup of Suit Host is completed
/// </summary>
public class SuitHostStartCompletionSource : TaskCompletionSource;

/// <summary>
///     Specifies the middleware collection of MobileSuit
/// </summary>
public interface ISuitMiddlewareCollection : IReadOnlyList<ISuitMiddleware>
{
    /// <summary>
    ///     Build Request delegate using middlewares in collection
    /// </summary>
    /// <returns></returns>
    SuitRequestDelegate BuildSuitRequestDelegate();
}

/// <summary>
///     Builder to build a MobileSuit host.
/// </summary>
public class SuitHostBuilder
{
    private readonly TaskRecorder _cancelTasks = new();

    /// <summary>
    /// </summary>
    /// <param name="args"></param>
    internal SuitHostBuilder(string[]? args)
    {
        // Configuration and static
        AppInfo.StartArgs = args ?? [];
        Services.AddSingleton<ISuitAppInfo>(AppInfo);
        Services.AddSingleton<IConfiguration>(Configuration);
        Configuration.AddEnvironmentVariables();

        // IO
        Services.AddSingleton<IParsingService>(Parsing);
        Services.AddSingleton<PromptFormatter>(PromptFormatters.BasicPromptFormatter);
        Services.AddSingleton<IIOHubConfigurator>(_ => { });
        Services.AddScoped<IIOHubYouShouldNeverUse, FourBitIOHub>();
        Services.AddScoped<IIOHub, RealIOHub>();


        // History, logging and tasking
        Services.AddLogging();
        Services.AddSingleton<ITaskRecorder>(_cancelTasks);
        Services.AddSingleton<ITaskService, TaskService>();
        Services.AddSingleton<IHistoryService, HistoryService>();
        Services.AddScoped<ISuitLogBucket, SuitLogBucket>();

        // Shells
        Services.AddSingleton<SuitHostShell>
            (sp => SuitHostShell.FromCommandServer(typeof(SuitCommandServer)));
        Services.AddSingleton
            (sp => SuitAppShell.FromClients(sp.GetKeyedServices<SuitShell>(SuitBuildUtils.SUIT_CLIENT_SHELL)));


        // System and Lifecycle
        Services.AddSingleton<ISuitExceptionHandler, SuitExceptionHandler>();
        Services.AddSingleton<SuitHostStartCompletionSource>();
        Services.AddSingleton<IMobileSuitHost, SuitHost>();
        Services.AddSingleton<ISuitContextFactory, SuitContextFactory>();
        Services.AddScoped<CancellationTokenSource>();
        Services.AddSingleton<SuitRequestDelegate>
        (
            sp => sp.GetRequiredService<ISuitMiddlewareCollection>().BuildSuitRequestDelegate()
        );
        Services.AddSingleton<IHostApplicationLifetime>
        (
            sp =>
                new SuitHostApplicationLifetime
                (
                    sp.GetRequiredService<SuitHostStartCompletionSource>(),
                    () =>
                    {
                        var taskRecorder = sp.GetRequiredService<TaskRecorder>();
                        taskRecorder.IsLocked = true;
                        return Task.WhenAll(taskRecorder);
                    }
                )
        );
        Services.AddScoped<ISuitCommandServer, SuitCommandServer>();
        Services.AddScoped<ISuitContextProperties, SuitContextProperties>();
    }

    /// <summary>
    ///     Copy constructor
    /// </summary>
    /// <param name="origin"></param>
    protected internal SuitHostBuilder(SuitHostBuilder origin)
    {
        AppInfo = origin.AppInfo;
        Configuration = origin.Configuration;
        Services = origin.Services;
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
    public void AddClient(SuitShell client) { Services.AddKeyedSingleton(SuitBuildUtils.SUIT_CLIENT_SHELL, client); }

    /// <summary>
    ///     Add a client shell to mobile suit
    /// </summary>
    /// <param name="serverType"></param>
    public void UseCommandServer<T>() where T : class, ISuitCommandServer
    {
        Services.AddSingleton<SuitHostShell>
            (sp => SuitHostShell.FromCommandServer(typeof(T)));
        Services.AddScoped<ISuitCommandServer, T>();
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
    public IMobileSuitHost Build()
    {
        return Services
              .AddSuitMiddlewares(WorkFlow)
              .BuildServiceProvider()
              .GetRequiredService<IMobileSuitHost>();
    }
}