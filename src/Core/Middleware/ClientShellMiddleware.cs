using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core.Services;

namespace PlasticMetal.MobileSuit.Core.Middleware
{
    /// <summary>
    /// Middleware to execute command over suit server shell.
    /// </summary>
    public class ServerClientMiddleware : ISuitMiddleware
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
            var client = context.ServiceProvider.GetRequiredService<SuitClientShell>();
            var task = client.Execute(context);
            if (context.Properties.ContainsKey(SuitBuildTools.SuitAsTask))
            {
                tasks.AddTask(task, context);
            }
            else
            {
                await task;
                if (context.Status == RequestStatus.NotHandled)
                {
                    context.Status = RequestStatus.CommandNotFound;
                }
            }

            await next(context);
        }
    }
}
