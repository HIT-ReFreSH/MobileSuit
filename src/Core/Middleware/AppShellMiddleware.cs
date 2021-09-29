using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core.Services;

namespace PlasticMetal.MobileSuit.Core.Middleware
{
    /// <summary>
    /// Middleware to execute command over suit server shell.
    /// </summary>
    public class AppShellMiddleware : ISuitMiddleware
    {
        /// <inheritdoc/>
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
            var client = context.ServiceProvider.GetRequiredService<SuitAppShell>();
            var task = client.Execute(context);
            var asTask = context.Properties.TryGetValue(SuitBuildTools.SuitCommandTarget, out var target) &&
                         target == SuitBuildTools.SuitCommandTargetAppTask;
            var forceClient = target == SuitBuildTools.SuitCommandTargetApp;
            if (asTask)
            {
                tasks.AddTask(task, context);
            }
            else
            {
                
                await task;
                if (forceClient && context.Status == RequestStatus.NotHandled)
                {
                    context.Status = RequestStatus.CommandNotFound;
                }
            }

            await next(context);
        }
    }
}
