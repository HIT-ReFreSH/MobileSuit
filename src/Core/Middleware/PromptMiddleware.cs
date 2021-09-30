using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core.Services;

namespace PlasticMetal.MobileSuit.Core.Middleware
{
    /// <summary>
    /// Middleware which provides the prompt output before user input.
    /// </summary>
    public class PromptMiddleware : ISuitMiddleware
    {
        /// <inheritdoc/>
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
                var tasks = context.ServiceProvider.GetRequiredService<ITaskService>();
                var history = context.ServiceProvider.GetRequiredService<IHistoryService>();
                var info = context.ServiceProvider.GetRequiredService<ISuitAppInfo>();
                var prompt = new List<PrintUnit>();
                if (tasks.RunningCount > 0)
                    prompt.Add(($"{Lang.Tasks}{tasks.RunningCount}", io.ColorSetting.SystemColor));
                if (!string.IsNullOrEmpty(info.AppName))
                    prompt.Add((info.AppName, io.ColorSetting.PromptColor));
                if (history.Response is not null)
                    prompt.Add((history.Response, io.ColorSetting.InformationColor));
                prompt.Add(history.Status switch
                {
                    RequestStatus.Ok or RequestStatus.NoRequest => (Lang.AllOK, io.ColorSetting.OkColor),
                    RequestStatus.Running => (Lang.Running, io.ColorSetting.WarningColor),
                    RequestStatus.CommandParsingFailure => (Lang.InvalidCommand, io.ColorSetting.ErrorColor),
                    RequestStatus.CommandNotFound => (Lang.MemberNotFound, io.ColorSetting.ErrorColor),
                    RequestStatus.Interrupt => (Lang.Interrupt, io.ColorSetting.ErrorColor),
                    _ => (Lang.OnError, io.ColorSetting.ErrorColor)
                });
                await io.WriteAsync(prompt, OutputType.Prompt);
            }

            await next(context);
        }
    }
}
