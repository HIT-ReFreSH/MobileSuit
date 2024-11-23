using System.Threading.Tasks;
using HitRefresh.MobileSuit.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using static HitRefresh.MobileSuit.Core.SuitBuildUtils;

namespace HitRefresh.MobileSuit.Core.Middleware;

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

        var tasks = context.ServiceProvider.GetRequiredService<ITaskService>();
        var force = context.Properties.TryGetValue
                        (SUIT_COMMAND_TARGET, out var target)
                 && target == SUIT_COMMAND_TARGET_HOST;
        var forceClient = target is SUIT_COMMAND_TARGET_APP or SUIT_COMMAND_TARGET_APP_TASK;
        if (forceClient)
        {
            await next(context);
            return;
        }

        var server = context.ServiceProvider.GetRequiredService<SuitHostShell>();
        await tasks.RunTaskImmediately(server.Execute(context));
        if (force && context.Status == RequestStatus.NotHandled) context.Status = RequestStatus.CommandNotFound;
        await next(context);
    }
}