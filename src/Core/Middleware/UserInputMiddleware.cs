using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static PlasticMetal.MobileSuit.SuitBuildTools;

namespace PlasticMetal.MobileSuit.Core.Middleware;

/// <summary>
///     Middleware which provides user input
/// </summary>
public class UserInputMiddleware : ISuitMiddleware
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

            var (request, control) = SplitCommandLine(originInput);
            for (var i = 0; i < control.Count; i++)
                switch (control[i][0])
                {
                    case '>':
                        if (++i < control.Count)
                            io.Output = new StreamWriter(File.OpenWrite(control[i]));
                        else
                            context.Status = RequestStatus.CommandParsingFailure;

                        break;
                    case '<':
                        if (++i < control.Count)
                            io.Input = new StreamReader(File.OpenRead(control[i]));
                        else
                            context.Status = RequestStatus.CommandParsingFailure;

                        break;
                    case '!':
                        context.Properties.Add(SuitCommandTarget, SuitCommandTargetApp);
                        break;
                    case '@':
                        context.Properties.Add(SuitCommandTarget, SuitCommandTargetHost);
                        break;
                    case '&':
                        context.Properties.Add(SuitCommandTarget, SuitCommandTargetAppTask);
                        break;
                    default:
                        context.Status = RequestStatus.CommandParsingFailure;
                        break;
                }

            if (request.Count == 0)
            {
                context.Status = RequestStatus.Ok;
                return;
            }

            context.Status = RequestStatus.NotHandled;
            context.Request = request.ToArray();
        }

        await next(context);
    }

    /// <summary>
    ///     Split a commandline string to args[] array.
    /// </summary>
    /// <param name="commandLine">commandline string</param>
    /// <returns>args[] array</returns>
    private static unsafe (IList<string>, IList<string>) SplitCommandLine(string commandLine)
    {
        if (string.IsNullOrEmpty(commandLine)) return (Array.Empty<string>(), Array.Empty<string>());
        var l = new List<string>();
        var ctl = new List<string>();
        /*
         * Node:
         * S0: WordStart
         * S1: IORedirect
         * S2: Word
         * S3: QuotesWord
         * S4: AfterBackslash
         * S5: Comment
         *
         * Edge
         * S0:
         *  &,!,@,<space>: S0
         *  <,>: S1
         *  #: S5
         *  ",': S3(S2)
         *  \: S4(S2)
         *  default: S2
         * S1:
         *  <space>: S2(S1)
         *  ",': S3(S2,S1)
         *  \: S4(S2,S1)
            #: S5
         *  default: S2(S1)
         * S2:
         *  <space>: pop
         *  ",': S3(S2)
         *  \: S4(S2)
         *  <,>: S1
         *  #: S5
         *  default: S2
         * S3:
         *  ",': pop
         *  \: S4(S3)
         *  default: S3
         * S4:
         *  default: pop
         * S5:
         *  default: S5
         */
        var status = (byte) 0;
        var quote = '\'';
        var stk = stackalloc byte[8];
        var stkptr = stk;

        var i = 0;
        var buf = stackalloc char[256];

        void SpaceCommit()
        {
            status = (byte) (stkptr == stk ? 0 : *--stkptr);
            (status == 1 ? ctl : l).Add(Regex.Unescape(new string(buf, 0, i)));
            status = 0;
            i = 0;
        }

        foreach (var c in commandLine.TakeWhile(c => status != 5))
            switch (status)
            {
                case 0:
                    switch (c)
                    {
                        case '&' or '!' or '@':
                            ctl.Add(c.ToString());
                            break;
                        case ' ':
                            break;
                        case '<' or '>':
                            status = 1;
                            ctl.Add(c.ToString());
                            break;
                        case '"' or '\'':
                            status = 3;
                            *stkptr++ = 2;
                            quote = c;
                            break;
                        case '\\':
                            status = 4;
                            *stkptr++ = 2;
                            buf[i++] = c;
                            break;
                        case '#':
                            status = 5;
                            break;
                        default:
                            status = 2;
                            buf[i++] = c;
                            break;
                    }

                    break;
                case 1:
                    switch (c)
                    {
                        case ' ':
                            *stkptr++ = 1;
                            status = 2;
                            break;
                        case '"' or '\'':
                            *stkptr++ = 1;
                            *stkptr++ = 2;
                            status = 3;
                            quote = c;
                            break;
                        case '\\':
                            *stkptr++ = 1;
                            *stkptr++ = 2;
                            status = 4;
                            buf[i++] = c;
                            break;
                        default:
                            *stkptr++ = 1;
                            status = 2;
                            buf[i++] = c;
                            break;
                    }

                    break;
                case 2:
                    switch (c)
                    {
                        case '\'' or '"':
                            *stkptr++ = 2;
                            status = 3;
                            quote = c;
                            break;
                        case '\\':
                            *stkptr++ = 2;
                            status = 4;
                            buf[i++] = c;
                            break;
                        case ' ':
                            SpaceCommit();
                            break;
                        case '#':
                            SpaceCommit();
                            status = 5;
                            break;
                        default:
                            buf[i++] = c;
                            break;
                    }

                    break;
                case 3:
                    if (c == quote)
                    {
                        status = *--stkptr;
                    }
                    else if (c == '\\')
                    {
                        *stkptr++ = 3;
                        status = 4;
                        buf[i++] = c;
                    }
                    else
                    {
                        buf[i++] = c;
                    }

                    break;
                case 4:
                    status = *--stkptr;
                    buf[i++] = c;
                    break;
            }

        if (status == 2)
            SpaceCommit();

        return (l, ctl);
    }
}