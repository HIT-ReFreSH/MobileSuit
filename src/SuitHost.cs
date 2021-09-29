#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core;
using Microsoft.Extensions.Logging;
using System.Linq;
using PlasticMetal.MobileSuit.Core.Services;
using System.Linq.Expressions;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     A entity, which serves the shell functions of a mobile-suit program.
    /// </summary>
    internal class SuitHost : IMobileSuitHost
    {
        private readonly IReadOnlyList<ISuitMiddleware> _suitApp;
        private readonly IServiceScope _rootScope;
        private readonly ISuitExceptionHandler _exceptionHandler;
        private readonly IHost _delegateHost;
        private IIOHub IO { get; }
        private CancellationTokenSource systemInterruption;
        public SuitHost(IServiceProvider services,IReadOnlyList<ISuitMiddleware> middleware)
        {

            Services = services;
            _suitApp = middleware;
            _exceptionHandler = Services.GetRequiredService<ISuitExceptionHandler>();
            _rootScope = Services.CreateScope();
            IO = _rootScope.ServiceProvider.GetRequiredService<IIOHub>();
            Logger = Services.GetRequiredService<ILogger<SuitHost>>();
            systemInterruption = new();
        }
        /// <inheritdoc/>
        public ILogger Logger{ get; }

        public void Dispose()
        {
            _rootScope.Dispose();
            _delegateHost.Dispose();
        }
        public async Task StartAsync(CancellationToken cancellationToken = new())
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            cancellationToken.Register(() =>
            {
                systemInterruption.Cancel();
            });


            var requestStack = new Stack<SuitRequestDelegate>();
            requestStack.Push( _ => Task.CompletedTask);
            SuitRequestDelegate last = _ => Task.CompletedTask;
            foreach (var middleware in _suitApp.Reverse())
            {
                var p = requestStack.Peek();
                Task Del(SuitContext c) => middleware.InvokeAsync(c, p);
                requestStack.Push(Del);
            }
            var handleRequest = last;
            var appInfo = _rootScope.ServiceProvider.GetRequiredService<ISuitAppInfo>();
            if (appInfo.StartArgs.Length > 0)
            {
                systemInterruption = new();
                var requestScope = Services.CreateScope();
                var context = new SuitContext(requestScope, systemInterruption);
                context.Status = RequestStatus.NotHandled;
                context.Request = appInfo.StartArgs;
                try
                {
                    await handleRequest(context);
                }
                catch (Exception ex)
                {
                    context.Exception = ex;
                    context.Status = RequestStatus.Faulted;
                    await _exceptionHandler.InvokeAsync(context);
                }
            }
            for (; ; )
            {
                systemInterruption = new();
                var requestScope = Services.CreateScope();
                var context = new SuitContext(requestScope, systemInterruption);
                try
                {
                    await handleRequest(context);
                }
                catch (Exception ex)
                {
                    context.Exception = ex;
                    context.Status = RequestStatus.Faulted;
                    await _exceptionHandler.InvokeAsync(context);
                    continue;
                }
                if (context.Status == RequestStatus.NoRequest && systemInterruption.IsCancellationRequested)
                {
                    break;
                }
            }

        }

        private void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            systemInterruption.Cancel();
        }

        public Task StopAsync(CancellationToken cancellationToken = new())
        {
            systemInterruption.Cancel();
            return Task.CompletedTask;
        }

        public IServiceProvider Services { get; }
    }
}