using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static HitRefresh.MobileSuit.Core.SuitBuildUtils;

namespace HitRefresh.MobileSuit.Core.Middleware;

/// <summary>
///     Middleware which provides user input
/// </summary>
public class IOInputMiddleware : ISuitMiddleware
{
    /// <inheritdoc />
    public async Task InvokeAsync(SuitContext context, SuitRequestDelegate next)
    {
        if (context.CancellationToken.IsCancellationRequested)
        {
            context.Status = RequestStatus.Interrupt;
            await next(context);
        }

        if (context.Status == RequestStatus.NoRequest)
        {
            var io = context.ServiceProvider.GetRequiredService<IIOHub>();
            var originInput = await io.ReadLineAsync();
            if (originInput is null)
            {
                context.Status = RequestStatus.OnExit;
                return;
            }

            if (originInput.StartsWith("#"))
            {
                context.Status = RequestStatus.Ok;
                return;
            }

            context.Request = [originInput];
        }

        await next(context);
    }



}