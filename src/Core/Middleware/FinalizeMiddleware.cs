using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core.Services;

namespace PlasticMetal.MobileSuit.Core.Middleware
{
    /// <summary>
    /// Middleware to finalize the command execution.
    /// </summary>
    public class FinalizeMiddleware : ISuitMiddleware
    {
        /// <inheritdoc/>
        public async Task InvokeAsync(SuitContext context, SuitRequestDelegate next)
        {
            var history = context.ServiceProvider.GetRequiredService<IHistoryService>();
            history.Response = context.Response;
            history.Status = context.Status;
            if (context.Status != RequestStatus.Running)
            {
                context.Dispose();
            }
            await next(context);
        }
    }
}
