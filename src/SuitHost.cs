#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Core.Services;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     A entity, which serves the shell functions of a mobile-suit program.
    /// </summary>
    internal class SuitHost : IMobileSuitHost
    {
        private readonly ISuitExceptionHandler _exceptionHandler;
        private readonly IServiceScope _rootScope;
        private readonly IReadOnlyList<ISuitMiddleware> _suitApp;
        private CancellationTokenSource _systemInterruption;

        public SuitHost(IServiceProvider services, IReadOnlyList<ISuitMiddleware> middleware)
        {
            Services = services;
            _suitApp = middleware;
            _exceptionHandler = Services.GetRequiredService<ISuitExceptionHandler>();
            _rootScope = Services.CreateScope();
            Logger = Services.GetRequiredService<ILogger<SuitHost>>();
            _systemInterruption = new CancellationTokenSource();
        }

        /// <inheritdoc />
        public ILogger Logger { get; }

        public IServiceProvider Services { get; }

        public void Dispose()
        {
            _rootScope?.Dispose();
        }


        public async Task StartAsync(CancellationToken cancellationToken = new())
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            cancellationToken.Register(() => { _systemInterruption.Cancel(); });


            var requestStack = new Stack<SuitRequestDelegate>();
            requestStack.Push(_ => Task.CompletedTask);
            SuitRequestDelegate last = _ => Task.CompletedTask;


            foreach (var middleware in _suitApp.Reverse())
            {
                var next = requestStack.Peek();
                requestStack.Push(async c => await middleware.InvokeAsync(c, next));
            }

            var handleRequest = requestStack.Peek();
            var appInfo = _rootScope.ServiceProvider.GetRequiredService<ISuitAppInfo>();
            if (appInfo.StartArgs.Length > 0)
            {
                _systemInterruption = new CancellationTokenSource();
                var requestScope = Services.CreateScope();
                var context = new SuitContext(requestScope, _systemInterruption);
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

            await HandleRequest(handleRequest);
        }

        public async Task StopAsync(CancellationToken cancellationToken = new())
        {
            _systemInterruption?.Cancel();
        }

        private async Task HandleRequest(SuitRequestDelegate requestHandler)
        {
            for (;;)
            {
                _systemInterruption = new CancellationTokenSource();
                var requestScope = Services.CreateScope();
                var context = new SuitContext(requestScope, _systemInterruption);
                try
                {
                    await requestHandler(context);
                }
                catch (Exception ex)
                {
                    context.Exception = ex;
                    context.Status = RequestStatus.Faulted;
                    await _exceptionHandler.InvokeAsync(context);
                    continue;
                }

                if (context.Status == RequestStatus.OnExit || context.Status == RequestStatus.NoRequest &&
                    _systemInterruption.IsCancellationRequested) break;
            }
        }

        private void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _systemInterruption.Cancel();
        }
    }
}