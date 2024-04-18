#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Services;

namespace HitRefresh.MobileSuit;

/// <summary>
///     A entity, which serves the shell functions of a mobile-suit program.
/// </summary>
internal class RequestQueueSuitHost : IMobileSuitHost
{
    private readonly ISuitExceptionHandler _exceptionHandler;
    private readonly AsyncServiceScope _rootScope;
    private readonly IReadOnlyList<ISuitMiddleware> _suitApp;
    private IHostApplicationLifetime _lifetime;
    private TaskCompletionSource _startUp;
    private readonly TaskRecorder _cancellationTasks;
    private SuitRequestDelegate? _requestHandler;
    private Task? _hostTask;
    private IRequestQueue _requestQueue;
    private bool _shutDown;

    public RequestQueueSuitHost
    (
        IServiceProvider services,
        IReadOnlyList<ISuitMiddleware> middleware,
        TaskCompletionSource startUp,
        TaskRecorder cancellationTasks
    )
    {
        Services = services;
        _suitApp = middleware;
        _startUp = startUp;
        _cancellationTasks = cancellationTasks;
        _lifetime = Services.GetRequiredService<IHostApplicationLifetime>();
        _exceptionHandler = Services.GetRequiredService<ISuitExceptionHandler>();
        _rootScope = Services.CreateAsyncScope();
        Logger = Services.GetRequiredService<ILogger<SuitHost>>();
        _requestQueue = Services.GetRequiredService<IRequestQueue>();
    }

    /// <inheritdoc />
    public ILogger Logger { get; }

    public IServiceProvider Services { get; }

    public void Dispose() { _rootScope.Dispose(); }

    public void Initialize()
    {
        if (_requestHandler != null) return;
        var requestStack = new Stack<SuitRequestDelegate>();
        requestStack.Push(_ => Task.CompletedTask);


        foreach (var middleware in _suitApp.Reverse())
        {
            var next = requestStack.Peek();
            requestStack.Push(async c => await middleware.InvokeAsync(c, next));
        }

        _requestHandler = requestStack.Peek();
    }

    public async Task StartAsync(CancellationToken cancellationToken = new())
    {
        if (_hostTask is not null) return;
        Initialize();
        if (cancellationToken.IsCancellationRequested) return;
        _startUp.SetResult();
        _hostTask = HandleRequest();
    }

    public async Task StopAsync(CancellationToken cancellationToken = new())
    {
        if (_hostTask is null) return;
        _shutDown = true;
        await _requestQueue.StopAsync();
    }

    private async Task HandleRequest()
    {
        if (_requestHandler is null) return;
        while (!_shutDown)
        {
            if (!_requestQueue.HasRequest)
            {
                await Task.Delay(_requestQueue.FetchPeriod);
            }

            var requestScope = Services.CreateScope();
            var context = new SuitContext(requestScope);
            var request = await _requestQueue.GetRequestAsync();
            context.Request = [request];
            try
            {
                await _requestHandler(context);
            }
            catch (Exception ex)
            {
                context.Exception = ex;
                context.Status = RequestStatus.Faulted;
                await _exceptionHandler.InvokeAsync(context);
            }

            if (context.CancellationToken.IsCancellationRequested) break;
        }

        _lifetime.StopApplication();
    }


    public async ValueTask DisposeAsync()
    {
        await _rootScope.DisposeAsync();
        _hostTask = null;
    }
}