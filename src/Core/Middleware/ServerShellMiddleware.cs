﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core.Services;
using static PlasticMetal.MobileSuit.SuitBuildTools;

namespace PlasticMetal.MobileSuit.Core.Middleware
{
    /// <summary>
    /// Middleware to execute command over suit server shell.
    /// </summary>
    public class ServerShellMiddleware : ISuitMiddleware
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
            var force = context.Properties.TryGetValue(SuitCommandTarget, out var target) &&
                        target == SuitCommandTargetServer;
            var forceClient = target == SuitCommandTargetClient;
            if (forceClient)
            {
                await next(context);
                return;
            }
            if (context.Properties.ContainsKey(SuitAsTask))
            {
                if (force)
                {
                    context.Status = RequestStatus.CommandParsingFailure;
                }
                await next(context);
                return;
            }
            var server=context.ServiceProvider.GetRequiredService<SuitServerShell>();
            await server.Execute(context);
            if (force && context.Status == RequestStatus.NotHandled)
            {
                context.Status = RequestStatus.CommandNotFound;
            }
            await next(context);
        }
    }
}
