using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core.Services;
using static PlasticMetal.MobileSuit.SuitBuildTools;

namespace PlasticMetal.MobileSuit.Core.Middleware
{
    /// <summary>
    ///     Middleware to execute command over suit server shell.
    /// </summary>
    public class HostShellMiddleware : ISuitMiddleware
    {
        /// <inheritdoc />
        public async Task InvokeAsync(SuitContext context, SuitRequestDelegate next)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                context.Status = RequestStatus.Interrupt;
                await next(context);
            }

            if (context.Status != RequestStatus.NotHandled)
            {
                await next(context);
                return;
            }

            var force = context.Properties.TryGetValue(SuitCommandTarget, out var target) &&
                        target == SuitCommandTargetHost;
            var forceClient = target is SuitCommandTargetApp or SuitCommandTargetAppTask;
            if (forceClient)
            {
                await next(context);
                return;
            }

            var server = context.ServiceProvider.GetRequiredService<SuitHostShell>();
            await server.Execute(context);
            if (force && context.Status == RequestStatus.NotHandled) context.Status = RequestStatus.CommandNotFound;
            await next(context);
        }
    }
}