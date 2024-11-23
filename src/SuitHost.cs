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
/// <param name="services">ServiceCollection</param>
/// <param name="startUp">Startup event</param>
/// <param name="requestHandler"></param>
/// <param name="contextFactory"></param>
/// <param name="lifetime"></param>
/// <param name="logger"></param>
public class SuitHost    (
    IServiceProvider services,
    SuitHostStartCompletionSource startUp,
    SuitRequestDelegate requestHandler,
    ISuitContextFactory contextFactory,
    IHostApplicationLifetime lifetime,
    ILogger logger
) : IMobileSuitHost
{
    /// <summary>
    /// Factory to create a suit context.
    /// </summary>
    protected ISuitContextFactory ContextFactory { get; } = contextFactory;

    /// <summary>
    /// Request Handler,
    /// </summary>
    protected SuitRequestDelegate RequestHandler { get; } = requestHandler;

    /// <summary>
    /// Specifies the task of suit host startup
    /// </summary>
    protected SuitHostStartCompletionSource StartUp { get; } = startUp;

    private Task? _hostTask;



    /// <summary>
    ///     Logger of given host
    /// </summary>
    public ILogger Logger { get; } =logger;

    /// <summary>
    ///     Service collection of given host
    /// </summary>
    public IServiceProvider Services { get; } = services;

    /// <inheritdoc />
    public void Dispose() { }

    /// <inheritdoc />
    public virtual async Task StartAsync(CancellationToken cancellationToken = new())
    {
        if (_hostTask is not null) return;
        Console.CancelKeyPress += StartTimeCancelKeyPress;

        var appInfo = Services.GetRequiredService<ISuitAppInfo>();
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
    public virtual Task StopAsync(CancellationToken cancellationToken = new())
    {
        if (_hostTask is null) return Task.CompletedTask;
        _hostTask = null;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync() { return ValueTask.CompletedTask; }

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

        lifetime.StopApplication();
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