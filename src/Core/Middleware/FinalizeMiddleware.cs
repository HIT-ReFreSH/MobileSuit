using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core.Services;

namespace PlasticMetal.MobileSuit.Core.Middleware;

/// <summary>
///     Middleware to finalize the command execution.
/// </summary>
public class FinalizeMiddleware : ISuitMiddleware
{
    /// <inheritdoc />
    public async Task InvokeAsync(SuitContext context, SuitRequestDelegate next)
    {
        if (context.Status == RequestStatus.NotHandled) context.Status = RequestStatus.CommandNotFound;
        var history = context.ServiceProvider.GetRequiredService<IHistoryService>();
        history.Response = context.Response;
        history.Status = context.Status switch
        {
            RequestStatus.Handled or RequestStatus.Ok => RequestStatus.Ok,
            RequestStatus.OnExit => RequestStatus.OnExit,
            RequestStatus.NotHandled => RequestStatus.CommandNotFound,
            RequestStatus.Running => RequestStatus.Running,
            var r => r
        };
        if (!context.Properties.TryGetValue(SuitBuildUtils.SuitCommandTarget, out var target) ||
            target != SuitBuildUtils.SuitCommandTargetAppTask)
            context.Dispose();
        await next(context);
    }
}