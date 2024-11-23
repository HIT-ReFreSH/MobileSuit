using System;
using System.Threading;
using System.Threading.Tasks;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HitRefresh.MobileSuit;

/// <summary>
///     A entity, which serves the shell functions of a mobile-suit program.
/// </summary>
public class SuitHost : IMobileSuitHost
{
    private readonly ISuitExceptionHandler _exceptionHandler;
    private readonly IHostApplicationLifetime _lifetime;
    /// <summary>
    /// Factory to create a suit context.
    /// </summary>
    protected ISuitContextFactory ContextFactory { get; }
    /// <summary>
    /// Request Handler,
    /// </summary>
    protected SuitRequestDelegate RequestHandler { get; }
    private readonly AsyncServiceScope _rootScope;
    /// <summary>
    /// Specifies the task of suit host startup
    /// </summary>
    protected SuitHostStartCompletionSource StartUp { get; }
    private Task? _hostTask;

    /// <summary>
    ///     Create a SuitHost instance
    /// </summary>
    /// <param name="services">ServiceCollection</param>
    /// <param name="startUp">Startup event</param>
    /// <param name="requestHandler"></param>
    /// <param name="contextFactory"></param>
    public SuitHost
    (
        IServiceProvider services,
        SuitHostStartCompletionSource startUp,
        SuitRequestDelegate requestHandler,
        ISuitContextFactory contextFactory
    )
    {
        Services = services;
        StartUp = startUp;
        _exceptionHandler = Services.GetRequiredService<ISuitExceptionHandler>();
        RequestHandler = requestHandler;
        ContextFactory = contextFactory;
        _lifetime = Services.GetRequiredService<IHostApplicationLifetime>();

        _rootScope = Services.CreateAsyncScope();
        Logger = Services.GetRequiredService<ILogger<SuitHost>>();
    }

    /// <summary>
    ///     Logger of given host
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    ///     Service collection of given host
    /// </summary>
    public IServiceProvider Services { get; }

    /// <inheritdoc />
    public void Dispose() { _rootScope.Dispose(); }

    /// <inheritdoc />
    public virtual async Task StartAsync(CancellationToken cancellationToken = new())
    {
        if (_hostTask is not null) return;
        Console.CancelKeyPress += StartTimeCancelKeyPress;

        var appInfo = _rootScope.ServiceProvider.GetRequiredService<ISuitAppInfo>();
        Console.CancelKeyPress -= StartTimeCancelKeyPress;
        if (cancellationToken.IsCancellationRequested) return;
        if (appInfo.StartArgs.Length > 0)
        {
            var context = ContextFactory.CreateContext();
            var cancelKeyHandler = CreateCancelKeyHandler(context);
            Console.CancelKeyPress += cancelKeyHandler;
            await RequestHandler(context);
            Console.CancelKeyPress -= cancelKeyHandler;
        }

        StartUp.SetResult();
        _hostTask = HandleRequest();
    }

    /// <inheritdoc />
    public virtual async Task StopAsync(CancellationToken cancellationToken = new())
    {
        if (_hostTask is null) return;
        _hostTask = null;
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() { await _rootScope.DisposeAsync(); }

    /// <summary>
    ///     Run middlewares in given order for a cycle
    /// </summary>
    protected async Task HandleRequest()
    {
        for (;;)
        {
            var context = ContextFactory.CreateContext();
            var cancelKeyHandler = CreateCancelKeyHandler(context);
            Console.CancelKeyPress += cancelKeyHandler;
            await RequestHandler(context);
            Console.CancelKeyPress -= cancelKeyHandler;
            if (context.Status == RequestStatus.OnExit
             || context is { Status: RequestStatus.NoRequest, CancellationToken.IsCancellationRequested: true }) break;
        }

        _lifetime.StopApplication();
    }

    private void StartTimeCancelKeyPress(object? sender, ConsoleCancelEventArgs e) { e.Cancel = true; }

    private static ConsoleCancelEventHandler CreateCancelKeyHandler(SuitContext context)
    {
        return (sender, e) =>
        {
            e.Cancel = true;
            context.CancellationToken.Cancel();
        };
    }
}